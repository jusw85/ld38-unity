using UnityEngine;
using UnityEngine.UI;
using InControl;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class EndingManager : MonoBehaviour {

    [System.Serializable]
    public class Ending {
        public Sprite sprite;
        public TextAsset textAsset;
    }

    public ScreenFader screenFader;
    public CameraZoomer cameraZoomer;
    public Text textBox;
    public float fadeDuration = 1f;
    public float initialZoomSize = 4f;
    public Vector3 initialCameraPos = new Vector3(4.2f, 0.75f, -10);

    public Ending[] endings;

    private SpriteRenderer spriteRenderer;
    private int idx = 0;
    private Actions actions;
    private bool isFadingIn = false;
    private Tween cameraMoveTween;
    private Tween girlFadeinTween;
    private Transform cam;
    private SpriteRenderer girl;

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        cam = Camera.main.transform;
        girl = transform.Find("Girl").GetComponent<SpriteRenderer>();


        NextScreen();
        cameraZoomer.Zoom(8.4375f, fadeDuration, zoomFromSize: initialZoomSize);
        cam.position = initialCameraPos;
        cameraMoveTween = DOTween
            .To(() => cam.position, x => cam.position = x, new Vector3(0f, 0f, -10f), fadeDuration)
            .Play();
        girlFadeinTween = DOTween
            .To(() => girl.color, x => girl.color = x, new Color(1f, 1f, 1f, 1f), fadeDuration)
            .Play();
        actions = Actions.CreateWithDefaultBindings();
    }

    public void Update() {
        if (actions.Enter.WasPressed) {
            if (!screenFader.IsFading() || isFadingIn) {
                if (isFadingIn) {
                    screenFader.CompleteFade();
                }
                if (cameraZoomer.IsZooming()) {
                    cameraZoomer.CompleteZoom();
                }
                if (cameraMoveTween.IsPlaying()) {
                    cameraMoveTween.Complete();
                }
                if (girlFadeinTween.IsPlaying()) {
                    girlFadeinTween.Complete();
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
        if (idx >= 1) {
            girl.gameObject.SetActive(false);
        }
        if (idx <= endings.Length - 1) {
            spriteRenderer.sprite = endings[idx].sprite;
            textBox.text = endings[idx].textAsset.text;
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
            actions.Enter.AddDefaultBinding(Mouse.LeftButton);
            actions.Enter.AddDefaultBinding(InputControlType.Action1);
            actions.Enter.AddDefaultBinding(InputControlType.Action2);
            actions.Enter.AddDefaultBinding(InputControlType.Action3);
            actions.Enter.AddDefaultBinding(InputControlType.Action4);
            return actions;
        }
    }
}



