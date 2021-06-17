﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueWindow : MonoBehaviour
{
    const string kAlphaCode = "<color=#00000000>";

    public TMP_Text dialogueText;
    string _currentText;
    CanvasGroup _canvasGroup;

    Image _currentImage;

    public bool isChatting;

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

        isChatting = true;
        _canvasGroup.alpha = 1;
        _currentText = text;
        StartCoroutine(DisplayText());    
    }

    public void Close()
    {
        StopAllCoroutines();
        isChatting= false;
        _canvasGroup.alpha = 0;
    }

    IEnumerator DisplayText()
    {
        dialogueText.text = "";

        string ogText = _currentText;
        string displayedText = "";

        int alphaIndex =0;

        foreach (var c in _currentText.ToCharArray())
        {
            alphaIndex++;
            dialogueText.text = ogText;
            displayedText = dialogueText.text.Insert(alphaIndex,kAlphaCode);

            dialogueText.text = displayedText;
            
            yield return UpdateManager.WaitForSecondsCustom(0.01f);
        }

        yield return null;
    }
}