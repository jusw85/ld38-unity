using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ScreenFader : MonoBehaviour {

    public Color fadeInColor = new Color(0f, 0f, 0f, 1f);
    public float fadeInSpeed = 0f;
    public Color fadeOutColor = new Color(0f, 0f, 0f, 1f);
    public float fadeOutSpeed = 1.5f;
    public bool startFadingIn = false;

    private Image flashImage;
    private Tween fadeInTween;
    private Tween fadeOutTween;

    public void Awake() {
        flashImage = GetComponentInChildren<Image>();
        fadeInTween = DOTween
            .To(() => flashImage.color, x => flashImage.color = x, Color.clear, fadeOutSpeed);
        fadeOutTween = DOTween
            .To(() => flashImage.color, x => flashImage.color = x, fadeOutColor, fadeOutSpeed);

        if (startFadingIn) {
            flashImage.color = fadeInColor;
            FadeIn();
        }
    }

    public bool IsFadeInPlaying() {
        return fadeInTween.IsPlaying();
    }

    public bool IsFadeOutPlaying() {
        return fadeOutTween.IsPlaying();
    }

    public void FadeIn() {
        fadeInTween.Play();
    }

    public void FadeOut() {
        fadeOutTween.Play();
    }

    public void OnFadeInComplete(TweenCallback onFadeInComplete) {
        fadeInTween.OnComplete(onFadeInComplete);
    }

    public void OnFadeOutComplete(TweenCallback onFadeOutComplete) {
        fadeOutTween.OnComplete(onFadeOutComplete);
    }
}
