using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Skip : MonoBehaviour {
    private Button button;

    void Start() {
        button = GetComponent<Button>();
        button.onClick.AddListener(SkipIntro);
    }

    private void SkipIntro() {
        SceneManager.LoadScene("TitleScreen");
    }
}
