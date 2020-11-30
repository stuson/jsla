using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour {
    [SerializeField] private BatterySlider batterySlider;
    [SerializeField] private float dischargeRate = 0.01f;
    public float charge = 1f;
    private float chargeSpeed = 0f;
    private PlayerMovement player;
    private SpriteRenderer render;
    private float initialPlayerSpeed;

    void Start() {
        render = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerMovement>();
        initialPlayerSpeed = player.speed;
    }

    void Update() {
        if (charge > 0f) {
            Discharge();
        }
    }

    private void Discharge() {
        charge = Mathf.Clamp01(charge - dischargeRate * Time.deltaTime);
        if (charge <= 0.6f) {
            float newValue = Mathf.InverseLerp(0f, 0.6f, charge);
            render.color = new Color(newValue, newValue, newValue);
        } else if (render.color.r < 1f) {
            render.color = new Color(1, 1, 1);
        }

        CheckCharge();
        batterySlider.UpdateCharge(charge);
    }

    private void CheckCharge() {
        if (charge <= 0f) {
            player.speed = 0f;
        }
    }

    public void Charge() {
        charge = Mathf.SmoothDamp(charge, 1f, ref chargeSpeed, 4f);
        batterySlider.UpdateCharge(charge);
        if (player.speed == 0f) {
            player.speed = initialPlayerSpeed;
        }
    }
}
