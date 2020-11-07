using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningPointer : MonoBehaviour {
    public Vector3 target;
    public Color color;
    private Camera cam;
    private SpriteRenderer render;
    private GameObject player;

    void Start() {
        render = GetComponent<SpriteRenderer>();
        color.a = 0.5f;
        render.color = color;
        cam = Camera.main;
        player = GameObject.FindGameObjectWithTag("Player");
        render.enabled = false;
    }

    void Update() {
        float vSize = cam.orthographicSize * 2;
        float hSize = vSize * Screen.width / Screen.height;
        Vector3 camPos = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);
        Bounds camBounds = new Bounds(camPos, new Vector3(hSize, vSize, 0f));

        if (!camBounds.Contains(target)) {
            transform.up = target - player.transform.position;
            transform.position = Vector3.Lerp(player.transform.position, camBounds.ClosestPoint(target), 0.8f);

            render.enabled = true;
        } else {
            render.enabled = false;
        }
    }
}
