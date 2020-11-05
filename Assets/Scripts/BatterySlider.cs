using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BatterySlider : MonoBehaviour {
    private TextMeshProUGUI text;
    private Slider slider;
    [SerializeField] private Image fill;
    [SerializeField] private Gradient fillGradient;
    
    void Start() {
        text = GetComponentInChildren<TextMeshProUGUI>();
        slider = GetComponent<Slider>();
    }

    public void UpdateCharge(float charge) {
        slider.value = charge;
        Color fillColor = fillGradient.Evaluate(charge);
        fill.color = fillColor;
        text.text = $"{charge:0%}";
    }
}
