using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private BatterySlider batterySlider;
    [SerializeField] private float dischargeRate = 0.01f;
    public float charge = 1f;
    void Start() {
        
    }

    void Update() {
        Discharge();
    }

    private void Discharge() {
        charge -= dischargeRate * Time.deltaTime;
        CheckCharge();
        batterySlider.UpdateCharge(charge);
    }

    private void CheckCharge() {
        if (charge <= 0f) {
            Debug.Log("GAME OVER");
            Time.timeScale = 0f;
        }
    }
}
