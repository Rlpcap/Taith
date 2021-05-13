using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : MonoBehaviour
{
    const string kAlphaCode = "<color=#00000000>";
    const float kMaxTextTime = 0.1f;

    public static int textSpeed = 2;
    public Text dialogueText;

    string _currentText;
    CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        _canvasGroup.alpha = 0;
    }

    public void ShowText(string text)
    {
        _canvasGroup.alpha = 1;
        _currentText = text;
        StartCoroutine(DisplayText());
    }

    public void Close()
    {
        StopAllCoroutines();
        _canvasGroup.alpha = 0;
    }

    IEnumerator DisplayText()
    {
        dialogueText.text = "";

        foreach (var c in _currentText.ToCharArray())
        {
            dialogueText.text += c;
            yield return new WaitForSecondsRealtime(0.04f);
        }

        yield return null;
    }
}
