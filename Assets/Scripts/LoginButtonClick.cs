using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButtonClick : MonoBehaviour {

    private ScreenFader screenFader;

    public void Start() {
        screenFader = GameObject.Find("ScreenFader").GetComponent<ScreenFader>();
        screenFader.OnFadeOutComplete(OnFadeOutComplete);
    }

    public void LoginButtonOnClick() {
        if (!screenFader.IsFadeOutPlaying()) {
            screenFader.FadeOut();
        }
    }

    public void OnFadeOutComplete() {
        SceneManager.LoadSceneAsync("Scratch");
    }
}
