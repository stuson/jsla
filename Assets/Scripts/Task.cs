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
    
    private SpriteRenderer render;

    void Start() {
        breakTimer = Random.Range(minCooldown, maxCooldown);
        render = GetComponent<SpriteRenderer>();
    }

    void Update() {
        breakTimer -= Time.deltaTime;
        CheckStatus();
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
        status = TaskStatus.repaired;
        render.color = Color.white;
    }
    private void SetBroken() {
        status = TaskStatus.broken;
        render.color = Color.yellow;
    }

    private void SetCritical() {
        render.color = Color.red;
        status = TaskStatus.critical;
    }

    private void SetDestroyed() {
        render.color = Color.black;
        status = TaskStatus.destroyed;
        SetGameOver();
    }

    private void SetGameOver() {
        Debug.Log("GAME OVER");
        Time.timeScale = 0;
    }
}

public enum TaskStatus {
    repaired,
    broken,
    critical,
    destroyed
}
