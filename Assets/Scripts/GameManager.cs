using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Cinemachine;

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
    private AudioSource music;
    public bool gameOver = false;
    public CinemachineTargetGroup targetGroup;

    void Start() {
        victoryTimer = TimeSpan.FromSeconds(victoryTime);
        victoryTimerDisplay = GameObject.FindGameObjectWithTag("VictoryTimer").GetComponent<TextMeshProUGUI>();
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu").GetComponent<CanvasGroup>();
        gravitySlider = GameObject.FindGameObjectWithTag("GravitySlider").GetComponent<GravitySlider>();
        warningNoise = GetComponent<AudioSource>();
        music = Camera.main.GetComponent<AudioSource>();
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
                ResumeGame();
                SceneManager.LoadScene("TitleScreen");
            }
            return;
        }

        victoryTimer = victoryTimer.Subtract(TimeSpan.FromSeconds(Time.deltaTime));
        victoryTimerDisplay.text = $"{victoryTimer:m\\:ss}";
        if (victoryTimer.TotalSeconds < 0f) {
            Victory();
        }

        difficultyFactor *= 1f + Time.deltaTime / 100;
        gravitySlider.UpdateGravity(difficultyFactor);
    }

    private void PauseGame() {
        Time.timeScale = 0;
        pauseMenu.alpha = 1;
        music.Pause();
        warningNoise.Pause();
        isPaused = true;
    }

    private void ResumeGame() {
        Time.timeScale = 1;
        pauseMenu.alpha = 0;
        music.UnPause();
        warningNoise.UnPause();
        isPaused = false;
    }

    public void StartWarning() {
        if (!warningNoisePlaying) {
            warningNoise.Play();
            warningNoisePlaying = true;
        }
    }

    public void CheckCritical() {
        Task[] tasks = GameObject.FindObjectsOfType<Task>();
        foreach(Task task in tasks) {
            if (task.status == TaskStatus.critical) return;
        }

        EndWarning();
    }

    public void EndWarning() {
        warningNoise.Stop();
        warningNoisePlaying = false;
    }

    public void GameOver(Task task) {
        if (gameOver) {
            return;
        }

        StartCoroutine("StartGameOver", task);
    }

    private IEnumerator StartGameOver(Task task) {
        targetGroup.AddMember(task.transform, 0.2f, 1f);
        task.flying = true;
        yield return new WaitForSeconds(10f);
        GoToTitleScreen();
    }

    private void Victory() {
        GoToTitleScreen();
    }

    private void GoToTitleScreen() {
        SceneManager.LoadScene("TitleScreen");
    }
}
