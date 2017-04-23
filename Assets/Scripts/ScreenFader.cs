using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenFader : MonoBehaviour {

    private Image image;
    private Tween tween;

    public void Awake() {
        image = GetComponentInChildren<Image>();
    }

    public void Fade(Color fadeToColor, float fadeToSpeed, TweenCallback callback = null, Color? fadeFromColor = null) {
        if (tween != null) {
            tween.Complete();
        }
        if (fadeFromColor != null) {
            image.color = fadeFromColor.Value;
        }
        tween = DOTween
            .To(() => image.color, x => image.color = x, fadeToColor, fadeToSpeed);
        if (callback != null) {
            tween.OnComplete(callback);
        }
        tween.Play();
    }

    public bool IsFading() {
        return (tween != null && tween.IsPlaying());
    }

    public void CompleteFade() {
        if (IsFading()) {
            tween.Complete();
        }
    }

}
