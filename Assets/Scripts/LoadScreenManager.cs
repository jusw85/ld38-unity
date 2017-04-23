using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenManager : MonoBehaviour {

    public ScreenFader screenFader;
    public float loadDelay = 5f;
    public Color fadeOutColor = Color.black;
    public float fadeOutSpeed = 1.5f;

    public void Start() {
        StartCoroutine(Loading());
    }

    public IEnumerator Loading() {
        yield return new WaitForSeconds(loadDelay);
        screenFader.Fade(fadeOutColor, fadeOutSpeed, OnFadeOutComplete);
    }

    public void OnFadeOutComplete() {
        SceneManager.LoadSceneAsync(3);
    }

}
