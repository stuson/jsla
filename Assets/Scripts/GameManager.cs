using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    public TimeSpan victoryTimer;
    public float difficultyFactor;
    private TextMeshProUGUI victoryTimerDisplay;
    private CanvasGroup pauseMenu;
    private bool isPaused = false;

    void Start() {
        victoryTimer = TimeSpan.FromSeconds(300f);
        victoryTimerDisplay = GameObject.FindGameObjectWithTag("VictoryTimer").GetComponent<TextMeshProUGUI>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<CanvasGroup>();
    }

    void Update() {
        if (!isPaused && Input.GetKeyDown("escape")) {
            Debug.Log("Pasued");
            PauseGame();
            return;
        }

        if (isPaused) {
            if (Input.GetKeyDown("space")) {
                ResumeGame();
            } else if (Input.GetKeyDown("escape")) {
                Application.Quit();
            }
            return;
        }

        victoryTimer = victoryTimer.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
        victoryTimerDisplay.text = $"{victoryTimer:m\\:ss}";
        if (victoryTimer.TotalSeconds < 0f) {
            GameOver();
        }
    }

    private void PauseGame() {
        Time.timeScale = 0;
        pauseMenu.alpha = 1;
        isPaused = true;
    }

    private void ResumeGame() {
        Time.timeScale = 1;
        pauseMenu.alpha = 0;
        isPaused = false;
    }

    public void GameOver() {
        Debug.Log("Game Over");
        SceneManager.LoadScene("TitleScreen");
    }
}
