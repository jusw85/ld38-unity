using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScreenManager : MonoBehaviour {

    public ScreenFader screenFader;
    public float loadDelay = 5f;

    public void Awake() {
        screenFader.OnFadeOutComplete(OnFadeOutComplete);
    }

    public void Start() {
        StartCoroutine(Loading());
    }

    public IEnumerator Loading() {
        yield return new WaitForSeconds(loadDelay);
        screenFader.FadeOut();
    }

    public void OnFadeOutComplete() {
        SceneManager.LoadSceneAsync(3);
    }

}
