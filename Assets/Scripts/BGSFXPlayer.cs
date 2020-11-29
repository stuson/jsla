using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSFXPlayer : MonoBehaviour {
    void Start() {
        BGSFXPlayer[] otherSfx = GameObject.FindObjectsOfType<BGSFXPlayer>();
        if (otherSfx.Length > 1) {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject.transform);
    }
}
