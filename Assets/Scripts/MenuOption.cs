using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MenuOption : MonoBehaviour, IPointerClickHandler {
    private SpriteRenderer indicator;
    public AudioSource selectNoise;

    void Start() {
        indicator = GetComponentInChildren<SpriteRenderer>();
        selectNoise = GetComponent<AudioSource>();
    }

    public void Select() {
        indicator.enabled = true;
    }

    public void Deselect() {
        indicator.enabled = false;
    }

    public void OnPointerClick(PointerEventData pointer) {
        Trigger();
    }

    abstract public void Trigger();
}
