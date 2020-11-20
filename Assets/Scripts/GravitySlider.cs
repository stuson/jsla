using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GravitySlider : MonoBehaviour {
    private Slider slider;
    
    void Start() {
        slider = GetComponent<Slider>();
    }

    public void UpdateGravity(float gravity) {
        slider.value = gravity;
    }
}
