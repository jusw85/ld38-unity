using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Chatbox : MonoBehaviour {

    private StringBuilder sb;
    private float textBoxHeight;
    public TextAsset chats;

    private string[] texts;
    private int textIdx = 0;

    public Text chatBox;
    public Text replyBox;
    public ControlManager cm;

    public EnemyController bunnyController;
    public Enemy bunny;
    public SpawnPointController sp1;
    public SpawnPointController sp2;
    public GameObject dk;

    public void Awake() {
        //chatBox = GetComponentInChildren<Text>();
        chatBox.text = "";
        replyBox.text = "";
        textBoxHeight = chatBox.rectTransform.rect.height;
        sb = new StringBuilder();

        texts = chats.text.Split('\n');
    }

    public void Start() {
        StartCoroutine(Start1());
        StartCoroutine(ChatSpam());
    }

    public void Update() {
        if (toCheckChatHeight) {
            if (CheckTextHeight()) {
                TruncateString();
            } else {
                toCheckChatHeight = false;
            }
        }
        if (inputPause && cm.isAttackPressed) {
            if (textIdx > 0) {
                sb.Append("\n");
            }
            sb.Append(replyBox.text);
            chatBox.text = sb.ToString();
            replyBox.text = "";
            StartCoroutine(DelayPause());
        }
    }

    public IEnumerator Start1() {
        while (bunny.currentHp > 0 || TargetControl.Instance.Enemies.Count > 0) {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(5f);
        fullPause = false;
    }

    public IEnumerator Start2() {
        while (TargetControl.Instance.Enemies.Count > 0) {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(5f);
        fullPause = false;
    }

    public IEnumerator Start3() {
        while (!bunnyController.isProximityStop) {
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(5f);
        fullPause = false;
    }

    public IEnumerator DelayPause() {
        yield return new WaitForSeconds(0.5f);
        inputPause = false;
    }

    private bool toCheckChatHeight = false;
    [System.NonSerialized]
    public bool inputPause = false;
    public bool fullPause = true;

    public GameObject npc;

    private IEnumerator ChatSpam() {
        while (textIdx < texts.Length) {
            while (fullPause) {
                yield return new WaitForSeconds(1f);
            }
            while (inputPause) {
                yield return new WaitForSeconds(1f);
            }
            string nxt = texts[textIdx++];
            if (nxt.StartsWith("zz8")) {
                yield return new WaitForSeconds(5f);
                GameObject go = GameObject.Find("UI Container");
                Destroy(go);
                go = GameObject.Find("(singleton) Toolbox");
                Destroy(go);
                SceneManager.LoadSceneAsync(4);

            } else if (nxt.StartsWith("zz7")) {
                fullPause = true;
                StartCoroutine(Start2());
                yield return new WaitForSeconds(1f);

            } else if (nxt.StartsWith("zz6")) {
                sp1.spawnInstances = 10;
                sp2.spawnInstances = 10;
                Instantiate(dk, bunnyController.transform.position, Quaternion.identity);
                bunnyController.Destroy();
                yield return new WaitForSeconds(1f);

            } else if (nxt.StartsWith("zz5")) {
                fullPause = true;
                StartCoroutine(Start2());
                yield return new WaitForSeconds(1f);

            } else if (nxt.StartsWith("zz4")) {
                bunnyController.isDying = true;
                yield return new WaitForSeconds(5f);
                sp1.spawnInstances = 5;
                sp2.spawnInstances = 5;

            } else if (nxt.StartsWith("zz2")) {
                bunnyController.followTarget = npc;
                bunnyController.followTargetController = null;
                fullPause = true;
                StartCoroutine(Start3());
                yield return new WaitForSeconds(1f);
            } else if (nxt.StartsWith("---")) {
                nxt = nxt.Remove(0, 4);
                replyBox.text = nxt;
                inputPause = true;
                yield return new WaitForSeconds(1f);
            } else {

                if (nxt.StartsWith("zzz")) {
                    bunny.maxHp = 10000;
                    bunnyController.isProximityStop = false;
                    bunnyController.OnObjectReuse();
                    bunnyController.Res();
                    nxt = nxt.Remove(0, 4);
                }
                if (nxt.Trim().Length > 0) {
                    if (CheckTextHeight()) {
                        toCheckChatHeight = true;
                        TruncateString();
                    }
                    if (textIdx > 0) {
                        sb.Append("\n");
                    }
                    //Debug.Log(texts[textIdx]);
                    sb.Append(nxt);
                    chatBox.text = sb.ToString();
                    yield return new WaitForSeconds(1f);
                }
            }
        }
    }

    private bool CheckTextHeight() {
        float textHeight = LayoutUtility.GetPreferredHeight(chatBox.rectTransform);
        return textHeight > textBoxHeight;
    }

    private void TruncateString() {
        bool isFound = false;
        for (int i = 0; i < sb.Length; i++) {
            if (sb[i] == '\n') {
                sb.Remove(0, i + 1);
                isFound = true;
                break;
            }
        }
        if (!isFound) {
            sb.Remove(0, sb.Length);
        }
        chatBox.text = sb.ToString();
    }

}
