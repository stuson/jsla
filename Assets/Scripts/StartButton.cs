using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MenuOption {
    public override void Trigger() {
        selectNoise.Play();
        StartCoroutine("StartGame");
    }

    public IEnumerator StartGame() {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main");
    }
}