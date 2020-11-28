using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSetting : MenuOption {
    private Slider volumeSlider;
    public AudioMixer mixer;
    public string mixerGroup;
    public AudioSource sampleNoise;
    public bool continuousSample = false;

    new void Start() {
        base.Start();
        volumeSlider = GetComponentInChildren<Slider>();
        volumeSlider.onValueChanged.AddListener(delegate { UpdateVolume(); });
        if (sampleNoise == null) {
            sampleNoise = selectNoise;
        }

        float currentVolume;
        mixer.GetFloat(mixerGroup, out currentVolume);
        currentVolume = Mathf.InverseLerp(-40f, 0f, currentVolume);
        SetVolume(currentVolume);
    }

    void Update() {
        if (selected && Input.GetButtonDown("Horizontal")) {
            float h = Input.GetAxisRaw("Horizontal");
            IncrementVolume(0.1f * h);
        }
    }

    public override void Trigger() {
        PlaySampleNoise();
    }

    private void PlaySampleNoise() {
        sampleNoise.Play();
    }

    private void IncrementVolume(float delta) {
        float currentVolume;
        mixer.GetFloat(mixerGroup, out currentVolume);
        currentVolume = Mathf.InverseLerp(-40f, 0f, currentVolume);
        float newVolume = Mathf.Clamp(currentVolume + delta, 0f, 1f);
        SetVolume(newVolume);
        PlaySampleNoise();
    }

    private void SetVolume(float volume) {
        mixer.SetFloat(mixerGroup, Mathf.Lerp(-40f, 0f, volume));
        volumeSlider.value = volume;
    }

    public void UpdateVolume() {
        float volume = volumeSlider.value;
        SetVolume(volume);
        if (!sampleNoise.isPlaying) {
            PlaySampleNoise();
        }
    }
}