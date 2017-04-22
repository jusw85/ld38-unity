using UnityEngine;
using UnityEngine.SceneManagement;
using InControl;

public class CharacterSelectManager : MonoBehaviour {

    public ScreenFader screenFader;
    private Actions actions;
    public int nextScene;

    public void Start() {
        screenFader.OnFadeOutComplete(OnFadeOutComplete);
        actions = Actions.CreateWithDefaultBindings();
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
        if (!screenFader.IsFadeOutPlaying()) {
            screenFader.FadeOut();
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



