using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour {
    public TimeSpan victoryTimer;
    public float victoryTime = 300f;
    public float difficultyFactor = 1f;
    private TextMeshProUGUI victoryTimerDisplay;
    private GravitySlider gravitySlider;
    private CanvasGroup pauseMenu;
    private bool isPaused = false;
    private AudioSource warningNoise;
    private bool warningNoisePlaying;

    void Start() {
        victoryTimer = TimeSpan.FromSeconds(victoryTime);
        victoryTimerDisplay = GameObject.FindGameObjectWithTag("VictoryTimer").GetComponent<TextMeshProUGUI>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<CanvasGroup>();
        gravitySlider = GameObject.FindGameObjectWithTag("GravitySlider").GetComponent<GravitySlider>();
        warningNoise = GetComponent<AudioSource>();
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

        difficultyFactor *= 1f + Time.deltaTime / 100;
        gravitySlider.UpdateGravity(difficultyFactor);
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

    public void StartWarning() {
        if (!warningNoisePlaying) {
            warningNoise.Play();
            warningNoisePlaying = true;
        }
    }

    public void EndWarning() {
        warningNoise.Stop();
        warningNoisePlaying = false;
    }

    public void GameOver() {
        Debug.Log("Game Over");
        SceneManager.LoadScene("TitleScreen");
    }
}
