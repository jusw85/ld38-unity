using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraZoomer : MonoBehaviour {
    
    public float zoomToSize = 8.4375f;
    public float zoomToSpeed = 1.5f;
    public bool startZoomingOut = false;

    private Camera cam;
    private Tween tween;

    public float finalOrthSize = 8.4375f;
    public float zoomSpeed;

    public void Start() {
        cam = Camera.main;

        tween = DOTween
            .To(() => cam.orthographicSize, x => cam.orthographicSize = x, finalOrthSize, zoomSpeed);
        tween.Play();
    }


}
