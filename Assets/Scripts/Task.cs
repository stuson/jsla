using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Task : MonoBehaviour {
    [SerializeField] private float minCooldown = 20f;
    [SerializeField] private float maxCooldown = 120f;
    private TaskStatus status = TaskStatus.repaired;

    [SerializeField] private float criticalThreshold = -160f;
    [SerializeField] private float gameOverThreshold = -180f;
    [SerializeField] private float breakTimer;

    private float originalX;
    
    private SpriteRenderer render;

    [SerializeField] private GameObject warningPointerBroken;
    [SerializeField] private GameObject warningPointerCritical;
    private Camera cam;
    private GameObject pointer;

    [SerializeField] private Sprite repairedSprite;
    [SerializeField] private Material repairedMaterial;
    [SerializeField] private Sprite brokenSprite;
    [SerializeField] private Material brokenMaterial;
    [SerializeField] private Sprite criticalSprite;
    [SerializeField] private Material criticalMaterial;
    private GameManager gameManager;
    

    void Start() {
        breakTimer = Random.Range(0f, 60f);
        render = GetComponent<SpriteRenderer>();
        originalX = transform.position.x;
        cam = Camera.main;
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    void Update() {
        breakTimer -= Time.deltaTime;
        CheckStatus();
        Shake();
    }

    public void Repair() {
        if (status != TaskStatus.repaired) {
            SetRepaired();
            breakTimer = Random.Range(minCooldown, maxCooldown);
        }
    }

    private void CheckStatus() {
        if (breakTimer > 0f) {
            return;
        } else if (status == TaskStatus.repaired && breakTimer < 0f) {
            SetBroken();
        } else if (status == TaskStatus.broken && breakTimer < criticalThreshold) {
            SetCritical();
        } else if (status == TaskStatus.critical && breakTimer < gameOverThreshold) {
            SetDestroyed();
            status = TaskStatus.broken;
        }
    }

    private void SetRepaired() {
        transform.position = new Vector3(originalX, transform.position.y, transform.position.z);
        status = TaskStatus.repaired;
        render.sprite = repairedSprite;
        render.material = repairedMaterial;
        RemovePointer();
    }
    private void SetBroken() {
        status = TaskStatus.broken;
        render.sprite = brokenSprite;
        render.material = brokenMaterial;
        CreatePointer(warningPointerBroken);
    }

    private void SetCritical() {
        status = TaskStatus.critical;
        render.sprite = criticalSprite;
        render.material = criticalMaterial;
        CreatePointer(warningPointerCritical);
    }

    private void SetDestroyed() {
        render.color = Color.black;
        status = TaskStatus.destroyed;
        gameManager.GameOver();
    }

    private void CreatePointer(GameObject warningPointer) {
        RemovePointer();
        pointer = Instantiate(warningPointer, Vector3.zero, Quaternion.identity, cam.transform);
        WarningPointer p = pointer.GetComponent<WarningPointer>();
        p.target = transform.position;
    }

    private void RemovePointer() {
        if (pointer != null) {
            Destroy(pointer);
            pointer = null;
        }
    }

    private void Shake() {
        float shakeAmount;
        float shakeSpeed;

        switch (status)
        {
            case TaskStatus.broken:
                shakeAmount = 0.05f;
                shakeSpeed = 20f;
                break;
            case TaskStatus.critical:
                shakeAmount = 0.1f;
                shakeSpeed = 80f;
                break;
            default:
                return;
        }

        transform.position = new Vector3(originalX + Mathf.Sin(Time.time * shakeSpeed) * shakeAmount, transform.position.y, transform.position.z);
    }
}

public enum TaskStatus {
    repaired,
    broken,
    critical,
    destroyed
}
