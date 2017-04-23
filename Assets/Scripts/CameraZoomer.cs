using UnityEngine;
using DG.Tweening;

public class CameraZoomer : MonoBehaviour {

    private Camera cam;
    private Tween tween;

    public void Awake() {
        cam = Camera.main;
    }

    public void Zoom(float zoomToSize, float zoomToSpeed, TweenCallback callback = null, float? zoomFromSize = null) {
        if (tween != null) {
            tween.Complete();
        }
        if (zoomFromSize != null) {
            cam.orthographicSize = zoomFromSize.Value;
        }
        tween = DOTween
            .To(() => cam.orthographicSize, x => cam.orthographicSize = x, zoomToSize, zoomToSpeed);
        if (callback != null) {
            tween.OnComplete(callback);
        }
        tween.Play();
    }

    public bool isZooming() {
        return (tween != null && tween.IsPlaying());
    }

}
