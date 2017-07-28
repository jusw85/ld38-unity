using UnityEngine;
using UnityEngine.UI;
using InControl;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EndingManager : MonoBehaviour {

    //[System.Serializable]
    //public class Ending {
    //    public Sprite sprite;
    //    public TextAsset textAsset;
    //}

    public ScreenFader screenFader;
    public CameraZoomer cameraZoomer;
    public Text textBox;
    public float fadeDuration = 1f;
    public float initialZoomSize = 4f;
    public Vector3 initialCameraPos = new Vector3(4.2f, 0.75f, -10);

    //public Ending[] endings;
    public Sprite[] endings;

    private SpriteRenderer spriteRenderer;
    private int idx = 0;
    private Actions actions;
    private bool isFadingIn = false;
    private Tween cameraMoveTween;
    private Tween girlFadeinTween;
    private Transform cam;
    private SpriteRenderer girl;

    public SpriteRenderer[] endingTexts;
    private int endingTextIdx = -1;

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main.transform;
        girl = transform.Find("Girl").GetComponent<SpriteRenderer>();

        NextScreen();
        actions = Actions.CreateWithDefaultBindings();

    }

    private Tween textTween;

    private void PlayTextTween() {
        textTween = DOTween
            .To(() => endingTexts[endingTextIdx].color,
            x => endingTexts[endingTextIdx].color = x,
            Color.white, fadeDuration)
            .Play();
        endingTextIdx++;
    }

    public void Update() {
        if (actions.Enter.WasPressed) {
            if (!screenFader.IsFading() || isFadingIn) {
                if (textTween != null && textTween.IsPlaying()) {
                    textTween.Complete();
                }
                if (isFadingIn) {
                    screenFader.CompleteFade();
                }
                if (cameraZoomer.IsZooming()) {
                    cameraZoomer.CompleteZoom();
                }
                if (cameraMoveTween != null && cameraMoveTween.IsPlaying()) {
                    cameraMoveTween.Complete();
                }
                if (girlFadeinTween != null && girlFadeinTween.IsPlaying()) {
                    girlFadeinTween.Complete();
                }

                if (idx == 1 && endingTextIdx < 3) {
                    PlayTextTween();
                    return;
                }

                if (idx == 3 && endingTextIdx < 4) {
                    PlayTextTween();
                    return;
                }
                if (idx == 4 && endingTextIdx < 5) {
                    PlayTextTween();
                    return;
                }
                if (idx == 5 && endingTextIdx < 7) {
                    PlayTextTween();
                    return;
                }

                if (idx <= endings.Length - 1) {
                    PreNextScreen();
                } else {
                    screenFader.Fade(Color.black, fadeDuration, Quit);
                }
            }
        }
    }

    public void PreNextScreen() {
        screenFader.Fade(Color.black, fadeDuration, NextScreen);
    }

    public void NextScreen() {
        if (idx == 1) {
            for (int i = 0; i < 4; i++) {
                endingTexts[i].enabled = false;
            }
            cameraZoomer.Zoom(8.4375f, fadeDuration, zoomFromSize: initialZoomSize);
            cam.position = initialCameraPos;
            cameraMoveTween = DOTween
                .To(() => cam.position, x => cam.position = x, new Vector3(0f, 0f, -10f), fadeDuration)
                .Play();
            girlFadeinTween = DOTween
                .To(() => girl.color, x => girl.color = x, new Color(1f, 1f, 1f, 1f), fadeDuration)
                .Play();
        }

        if (idx == 3) {
            girl.gameObject.SetActive(false);
            endingTexts[4].enabled = false;
        }
        if (idx == 4) {
            endingTexts[5].enabled = false;
        }
        if (idx == 5) {
            endingTexts[6].enabled = false;
            endingTexts[7].enabled = false;
        }
        if (idx <= endings.Length - 1) {
            spriteRenderer.sprite = endings[idx];
            //spriteRenderer.sprite = endings[idx].sprite;
            //textBox.text = endings[idx].textAsset.text;
            idx++;
        }
        PostNextScreen();
    }

    public void PostNextScreen() {
        isFadingIn = true;
        screenFader.Fade(Color.clear, fadeDuration, () => { isFadingIn = false; }, Color.black);
    }

    public void Quit() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private class Actions : PlayerActionSet {
        public PlayerAction Enter;

        public Actions() {
            Enter = CreatePlayerAction("Enter");
        }

        //http://www.gallantgames.com/pages/incontrol-standardized-controls
        // Action1 = A, X
        // Action2 = B, Circle
        // Action3 = X, Square
        // Action4 = Y, Triangle
        public static Actions CreateWithDefaultBindings() {
            var actions = new Actions();
            actions.Enter.AddDefaultBinding(Key.Return);
            actions.Enter.AddDefaultBinding(Key.J);
            actions.Enter.AddDefaultBinding(Mouse.LeftButton);
            actions.Enter.AddDefaultBinding(InputControlType.Action1);
            actions.Enter.AddDefaultBinding(InputControlType.Action2);
            actions.Enter.AddDefaultBinding(InputControlType.Action3);
            actions.Enter.AddDefaultBinding(InputControlType.Action4);
            return actions;
        }
    }
}



