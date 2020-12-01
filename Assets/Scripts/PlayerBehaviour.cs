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
    public AudioSource chargeNoise;
    private float currentVolume = 0f;
    private float targetVolume = 0f;
    private float fadeVelocity = 0f;

    void Start() {
        render = GetComponent<SpriteRenderer>();
        player = GetComponent<PlayerMovement>();
        initialPlayerSpeed = player.speed;
        chargeNoise = GetComponent<AudioSource>();
    }

    void Update() {
        if (charge > 0f) {
            Discharge();
        }

        if ((targetVolume == 0f && currentVolume > 0.001f) || (targetVolume == 1f && currentVolume < 0.999f)) {
            currentVolume = Mathf.SmoothDamp(currentVolume, targetVolume, ref fadeVelocity, 0.2f);
            chargeNoise.volume = currentVolume;
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
        targetVolume = 1f;
        if (player.speed == 0f) {
            player.speed = initialPlayerSpeed;
        }
    }

    public void StopCharge() {
        targetVolume = 0f;
    }
}
