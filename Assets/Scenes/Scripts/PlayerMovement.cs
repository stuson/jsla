using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [SerializeField] private float speed = 8f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float climbSpeed = 6.8f;
    [SerializeField] private float climbAcceleration = 4.4f;
    private float hVelocity;
    private float vVelocity;

    private BoxCollider2D collide;
    [SerializeField] private LayerMask ladderLayers;
    private ContactFilter2D ladderFilter;
    [SerializeField] private LayerMask groundLayers;
    private ContactFilter2D groundFilter;

    private bool isClimbing = false;
    private bool canDismount = false;

    void Start() {
        collide = GetComponent<BoxCollider2D>();
        ladderFilter = new ContactFilter2D();
        ladderFilter.SetLayerMask(ladderLayers);
        groundFilter = new ContactFilter2D();
        groundFilter.SetLayerMask(groundLayers);
    }

    void Update() {
        float hDirection = Input.GetAxisRaw("Horizontal");
        float vDirection = Input.GetAxisRaw("Vertical");

        if (isClimbing) {
            if ((canDismount || vDirection == 0) && hDirection != 0) {
                Collider2D ground = getOverlappingGround();
                if (ground != null) {
                    getOffLadder(ground, hDirection);
                }
            }

            Collider2D ladder = getOverlappingLadder(vDirection);
            if (ladder != null) {
                vVelocity = Mathf.MoveTowards(vVelocity, vDirection * climbSpeed, climbAcceleration);
            } else {
                vVelocity = 0f;
            }
            
            if (Input.GetButtonUp("Horizontal")) {
                canDismount = true;
            }
        } else {
            if (vDirection != 0) {
                Collider2D ladder = getOverlappingLadder(vDirection);
                if (ladder != null) {
                    getOnLadder(ladder);
                }
            }

            if (!isClimbing && hDirection != 0f && isGrounded(hDirection)) {
                hVelocity = Mathf.MoveTowards(hVelocity, hDirection * speed, acceleration);
            } else {
                hVelocity = 0f;
            }
        }

        transform.Translate(hVelocity * Time.deltaTime, vVelocity * Time.deltaTime, 0f);
    }

    private bool isGrounded(float direction) {
        Vector3 origin = new Vector3(transform.position.x + direction * transform.localScale.x / 2, transform.position.y - transform.localScale.y / 2, transform.position.z);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 0.01f, groundLayers);
        if (hit.collider != null) {
            return true;
        }

        return false;
    }

    private Collider2D getOverlappingGround() {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 4, transform.position.z);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, 3 * transform.localScale.y / 4 + 0.05f, groundLayers);
        return hit.collider;
    }

    private Collider2D getOverlappingLadder(float direction) {
        Vector2 point = new Vector2(transform.position.x, transform.position.y + direction * (transform.localScale.y / 2 + 0.01f));
        Collider2D ladder = Physics2D.OverlapPoint(point, ladderLayers);
        return ladder;
    }

    private void getOnLadder(Collider2D ladder) {
        hVelocity = 0f;
        transform.position = new Vector3(ladder.transform.position.x, transform.position.y, transform.position.z);
        isClimbing = true;
        canDismount = false;
    }

    private void getOffLadder(Collider2D ground, float direction) {
        vVelocity = 0f;
        float groundSurfaceHeight = ground.transform.position.y + ground.transform.localScale.y / 2;
        float playerHeightOffset = transform.localScale.y / 2;
        transform.position = new Vector3(transform.position.x + direction * transform.localScale.x / 2, groundSurfaceHeight + playerHeightOffset, transform.position.z);
        isClimbing = false;
    }
}
