using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class CharacterSelectManager : MonoBehaviour {

    public ScreenFader screenFader;
    public CameraZoomer cameraZoomer;
    public int nextScene;
    public AudioClip buttonSound;

    public float zoomOutSize;
    public float zoomOutSpeed;
    public float zoomInSize;
    public float zoomInSpeed;

    public Color fadeInColor;
    public float fadeInSpeed;
    public Color fadeOutColor;
    public float fadeOutSpeed;

    private Actions actions;
    private AudioSource audioSource;
    private bool isExiting = false;

    public void Awake() {
        actions = Actions.CreateWithDefaultBindings();
        audioSource = GetComponent<AudioSource>();
    }

    public void Start() {
        screenFader.Fade(fadeInColor, fadeInSpeed, fadeFromColor: fadeOutColor);
        cameraZoomer.Zoom(zoomOutSize, zoomOutSpeed, zoomFromSize: zoomInSize);
    }

    public void LoginButtonOnClick() {
        Login();
    }

    public void Update() {
        if (actions.Enter.WasPressed) {
            Login();
        }
    }

    public void Login() {
        if (!audioSource.isPlaying) {
            audioSource.PlayOneShot(buttonSound);
        }
        if (!isExiting) {
            isExiting = true;
            cameraZoomer.Zoom(zoomInSize, zoomInSpeed);
            screenFader.Fade(fadeOutColor, fadeOutSpeed, OnFadeOutComplete);
        }
    }

    public void OnFadeOutComplete() {
        SceneManager.LoadSceneAsync(nextScene);
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
            actions.Enter.AddDefaultBinding(InputControlType.Action1);
            actions.Enter.AddDefaultBinding(InputControlType.Action2);
            actions.Enter.AddDefaultBinding(InputControlType.Action3);
            actions.Enter.AddDefaultBinding(InputControlType.Action4);
            return actions;
        }
    }
}



