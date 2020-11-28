using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class MenuOption : MonoBehaviour, IPointerClickHandler {
    private SpriteRenderer indicator;
    public AudioSource selectNoise;
    public bool selected = false;

    public void Start() {
        indicator = GetComponentInChildren<SpriteRenderer>();
        if (selectNoise == null) {
            selectNoise = GetComponent<AudioSource>();
        }
    }

    public void Select() {
        indicator.enabled = true;
        selected = true;
    }

    public void Deselect() {
        indicator.enabled = false;
        selected = false;
    }

    public void OnPointerClick(PointerEventData pointer) {
        Trigger();
    }

    abstract public void Trigger();
}
