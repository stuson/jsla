using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour {
    public TimeSpan victoryTimer;
    public float difficultyFactor;
    private TextMeshProUGUI victoryTimerDisplay;

    void Start() {
        victoryTimer = TimeSpan.FromSeconds(300f);
        victoryTimerDisplay = GameObject.FindGameObjectWithTag("VictoryTimer").GetComponent<TextMeshProUGUI>();
    }

    void Update() {
        victoryTimer = victoryTimer.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
        victoryTimerDisplay.text = $"{victoryTimer:m\\:ss}";
        if (victoryTimer.TotalSeconds < 0f) {
            GameOver();
        }
    }

    public void GameOver() {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
    }
}
