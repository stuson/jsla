using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MenuOption {
    private bool loading = false;

    public override void Trigger() {
        if (loading) {
            return;
        }

        selectNoise.Play();
        loading = true;
        StartCoroutine("StartGame");
    }

    public IEnumerator StartGame() {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Main");
    }
}