using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public float victoryTimer = 600f;
    public float difficultyFactor;

    void Update() {
        victoryTimer -= Time.deltaTime;
        if (victoryTimer <= 0f) {
            GameOver();
        }
    }

    public void GameOver() {
        Debug.Log("Game Over");
        Time.timeScale = 0f;
    }
}
