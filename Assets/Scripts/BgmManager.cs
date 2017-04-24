using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgmManager : MonoBehaviour {

    private static BgmManager instance;
    public static BgmManager Instance { get { return instance; } }

    public AudioClip town;
    public AudioClip battle;
    public AudioClip whatsapp;

    private AudioSource source;
    private EventManager em;

    private void Awake() {
        if (instance != null && instance != this) {
            Destroy(gameObject);
        } else {
            instance = this;
        }
        source = GetComponent<AudioSource>();
        em = Toolbox.GetOrAddComponent<EventManager>();
    }

    public void PlayTown() {
        PlaySfxEvent ev = new PlaySfxEvent(town);
        em.Publish(Events.PLAY_BGM, ev);
    }

    public void PlayBattle() {
        PlaySfxEvent ev = new PlaySfxEvent(battle);
        em.Publish(Events.PLAY_BGM, ev);
    }

    public void PlayWhatsapp() {
        source.PlayOneShot(whatsapp);
        //PlaySfxEvent ev = new PlaySfxEvent(whatsapp);
        //em.Publish(Events.PLAY_SFX, ev);
    }
}
