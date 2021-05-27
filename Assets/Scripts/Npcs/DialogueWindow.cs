using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueWindow : MonoBehaviour
{
    public Text dialogueText;
    string _currentText;
    CanvasGroup _canvasGroup;

    Image _currentImage;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _currentImage = GetComponent<Image>();

    }

    private void Start()
    {
        _canvasGroup.alpha = 0;
    }

    public void ShowText(string text, Sprite image)
    {
        if(image!=null)
        _currentImage.sprite = image;

        
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
            yield return UpdateManager.WaitForSecondsCustom(0.04f);
            //yield return new WaitForSecondsRealtime(0.04f);
        }

        yield return null;
    }
}
