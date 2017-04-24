using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chatbox : MonoBehaviour {

    private StringBuilder sb;
    private float textBoxHeight;
    public TextAsset chats;

    private string[] texts;
    private int textIdx = 0;

    public Text chatBox;
    public Text replyBox;
    public ControlManager cm;

    public void Awake() {
        //chatBox = GetComponentInChildren<Text>();
        chatBox.text = "";
        replyBox.text = "";
        textBoxHeight = chatBox.rectTransform.rect.height;
        sb = new StringBuilder();

        texts = chats.text.Split('\n');
    }

    public void Start() {
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
        if (pauseText && cm.isAttackPressed) {
            if (textIdx > 0) {
                sb.Append("\n");
            }
            sb.Append(replyBox.text);
            chatBox.text = sb.ToString();
            replyBox.text = "";
            StartCoroutine(DelayPause());
        }
    }

    public IEnumerator DelayPause() {
        yield return new WaitForSeconds(0.5f);
        pauseText = false;
    }

    private bool toCheckChatHeight = false;
    [System.NonSerialized]
    public bool pauseText = false;
    public bool fullPause = true;

    private IEnumerator ChatSpam() {
        while (textIdx < texts.Length) {
            while (fullPause) {
                yield return new WaitForSeconds(1f);
            }
            while (pauseText) {
                yield return new WaitForSeconds(1f);
            }
            string nxt = texts[textIdx++];
            if (nxt.StartsWith("--")) {
                nxt = nxt.Remove(0, 3);
                replyBox.text = nxt;
                pauseText = true;
                yield return new WaitForSeconds(1f);
            } else {
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
