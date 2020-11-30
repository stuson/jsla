using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreenBlocker : MonoBehaviour {
    private Image image;
    private float alpha = 1f;
    private float alphaChange = 0f;

    void Start() {
        image = GetComponent<Image>();
    }

    void Update() {
        alpha = Mathf.SmoothDamp(alpha, 0f, ref alphaChange, 4f);
        image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
        
        if (alpha < 0.6f) {
            image.raycastTarget = false;
        } else if (alpha <= 0f) {
            Destroy(gameObject);
        }
    }
}
