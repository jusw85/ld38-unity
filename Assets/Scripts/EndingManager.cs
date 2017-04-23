using UnityEngine;
using UnityEngine.UI;
using InControl;
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

    public Ending[] endings;

    private SpriteRenderer spriteRenderer;
    private int idx = 0;
    private Actions actions;

    public void Start() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        NextScreen();
        actions = Actions.CreateWithDefaultBindings();
    }

    public void Update() {
        if (actions.Enter.WasPressed) {
            if (!NextScreen()) {
                Quit();
            }
        }
    }

    public bool NextScreen() {
        if (idx <= endings.Length - 1) {
            spriteRenderer.sprite = endings[idx].sprite;
            textBox.text = endings[idx].textAsset.text;
            idx++;
            return true;
        }
        return false;
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



