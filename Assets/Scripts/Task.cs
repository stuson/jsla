﻿using System.Collections;
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

    void Start() {
        breakTimer = Random.Range(minCooldown, maxCooldown);
        render = GetComponent<SpriteRenderer>();
        originalX = transform.position.x;
        Debug.Log(originalX);
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
        status = TaskStatus.repaired;
        render.color = Color.white;
        transform.position = new Vector3(originalX, transform.position.y, transform.position.z);
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
