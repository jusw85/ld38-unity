using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chatbox : MonoBehaviour {

    private Text textBox;
    private StringBuilder sb;
    private float textBoxHeight;

    public void Awake() {
        textBox = GetComponentInChildren<Text>();
        textBox.text = "";
        textBoxHeight = textBox.rectTransform.rect.height;
        sb = new StringBuilder();
    }

    public void Start() {
        StartCoroutine(ChatSpam());
    }

    //public void Update() {
    //}

    private int i = 0;

    private IEnumerator ChatSpam() {
        while (true) {
            if (CheckTextHeight()) {
                TruncateString();
            }
            if (i > 0) {
                sb.Append("\n");
            }
            sb.Append("Hello World " + i++);
            textBox.text = sb.ToString();
            yield return new WaitForSeconds(0.25f);
        }
    }

    private bool CheckTextHeight() {
        float textHeight = LayoutUtility.GetPreferredHeight(textBox.rectTransform);
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
        textBox.text = sb.ToString();
    }

}
