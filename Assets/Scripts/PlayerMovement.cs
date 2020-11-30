using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] public float speed = 14f;
    [SerializeField] private float acceleration = 2f;
    [SerializeField] private float climbSpeed = 8.8f;
    [SerializeField] private float climbAcceleration = 6.4f;
    private float hVelocity;
    private float vVelocity;

    private BoxCollider2D collide;
    private PlayerBehaviour player;

    [SerializeField] private LayerMask ladderLayers;
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private LayerMask taskLayers;
    [SerializeField] private LayerMask chargeStationLayers;

    private bool isClimbing = false;
    public bool canDismount = false;
    public bool canMount = true;
    private GameManager gameManager;

    void Start() {
        collide = GetComponent<BoxCollider2D>();
        player = GetComponent<PlayerBehaviour>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update() {
        if (isClimbing) {
            LadderMovement();
        } else {
            GroundMovement();
        }

        if (Input.GetButtonUp("Vertical")) {
            canDismount = true;
        }

        if (Input.GetButtonUp("Horizontal")) {
            canMount = true;
        }

        if (!gameManager.gameOver && Input.GetButtonDown("Interact")) {
            Collider2D taskCollider = GetOverlappingTask();
            if (taskCollider != null) {
                taskCollider.gameObject.GetComponent<Task>().Repair();
            }
        }

        if (!gameManager.gameOver && Input.GetAxis("Interact") > 0f && GetOverlappingChargeStation()) {
            player.Charge();
        }

        float speedMul = Mathf.Lerp(0.3f, 1f, player.charge);
        transform.Translate(hVelocity * speedMul * Time.deltaTime, vVelocity * speedMul * Time.deltaTime, 0f);
    }

    private void LadderMovement() {
        float hDirection = Input.GetAxisRaw("Horizontal");
        float vDirection = Input.GetAxisRaw("Vertical");

        Collider2D ground = GetOverlappingGround();
        Collider2D ladder = GetOverlappingLadder(vDirection);

        if (ground == null) {
            canDismount = true;
        }

        if (canDismount && hDirection != 0 && ground != null) {
            GetOffLadder(ground, hDirection);
            return;
        }

        if (vDirection != 0 && ladder != null) {
            vVelocity = Mathf.MoveTowards(vVelocity, vDirection * climbSpeed, climbAcceleration);
        } else {
            vVelocity = 0f;
        }
    }

    private void GroundMovement() {
        float hDirection = Input.GetAxisRaw("Horizontal");
        float vDirection = Input.GetAxisRaw("Vertical");

        Collider2D ground = GetOverlappingGround();
        Collider2D ladder = GetOverlappingLadder(vDirection);
        if (ladder == null) {
            canMount = true;
        }

        if (canMount && vDirection != 0 && ladder != null) {
            GetOnLadder(ladder);
            return;
        }

        if (hDirection != 0 && IsGrounded(hDirection)) {
            hVelocity = Mathf.MoveTowards(hVelocity, hDirection * speed, acceleration);
        } else {
            hVelocity = 0f;
        }
    }

    private bool IsGrounded(float direction) {
        Vector3 origin = new Vector3(transform.position.x + direction * transform.localScale.x / 2, transform.position.y - transform.localScale.y / 2, transform.position.z);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.01f, groundLayers);
        if (hit.collider != null) {
            return true;
        }

        return false;
    }

    private Collider2D GetOverlappingGround() {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, transform.localScale.y + 0.05f, groundLayers);

        Debug.DrawLine(origin, origin + Vector3.down * (transform.localScale.y + 0.05f), Color.red, Time.deltaTime, false);

        return hit.collider;
    }

    private Collider2D GetOverlappingLadder(float direction) {
        Vector2 point = new Vector2(transform.position.x, transform.position.y + direction * (transform.localScale.y / 2 + 0.01f));
        Collider2D ladder = Physics2D.OverlapPoint(point, ladderLayers);
        return ladder;
    }

    private Collider2D GetOverlappingTask() {
        Collider2D task = Physics2D.OverlapPoint(transform.position, taskLayers);
        return task;
    }

    private Collider2D GetOverlappingChargeStation() {
        Collider2D station = Physics2D.OverlapPoint(transform.position, chargeStationLayers);
        return station;
    }

    private void GetOnLadder(Collider2D ladder) {
        hVelocity = 0f;
        transform.position = new Vector3(ladder.transform.position.x, transform.position.y, transform.position.z);
        isClimbing = true;
        canDismount = false;
    }

    private void GetOffLadder(Collider2D ground, float direction) {
        vVelocity = 0f;
        float groundSurfaceHeight = ground.transform.position.y + ground.transform.localScale.y / 2;
        float playerHeightOffset = transform.localScale.y / 2;
        transform.position = new Vector3(transform.position.x + direction * (transform.localScale.x + 0.6f), groundSurfaceHeight + playerHeightOffset, transform.position.z);
        isClimbing = false;
        canMount = false;
    }
}
