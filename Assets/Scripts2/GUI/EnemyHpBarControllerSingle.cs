using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyHpBarControllerSingle : MonoBehaviour {

    private Slider slider;
    private EventManager eventManager;

    private GameObject bg;
    private Coroutine co;
    private void Awake() {

        slider = GetComponent<Slider>();
        bg = transform.Find("Background").gameObject;
        bg.SetActive(false);
        //eventManager = Toolbox.GetOrAddComponent<EventManager>();
    }


    public void SetValue(float current, float max) {
        if (co != null)
            StopCoroutine(co);
        bg.SetActive(true);
        slider.value = Mathf.Clamp((current / max), 0, 1);
        co = StartCoroutine(Fade());
    }

    public void SetRawValue(float val) {
        slider.value = val;
    }

    public IEnumerator Fade() {
        yield return new WaitForSeconds(1f);
        bg.SetActive(false);
    }

    //private void OnEnable() {
    //    eventManager.AddSubscriber(Events.PLAYER_HPCHANGE, HpChangeHandler);
    //}

    //private void OnDisable() {
    //    eventManager.RemoveSubscriber(Events.PLAYER_HPCHANGE, HpChangeHandler);
    //}

    //public void HpChangeHandler(IGameEvent e) {
    //    PlayerHpChangeEvent ev = (PlayerHpChangeEvent)e;
    //    slider.value = Mathf.Clamp((ev.currentHp / ev.maxHp), 0, 1);
    //}

}
