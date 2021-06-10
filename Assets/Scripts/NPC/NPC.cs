using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [TextArea]
    public string dialogueText;

    public DialogueWindow dialogueWindow;

    public Sprite npcImage;

    public override void Interact()
    {
        if (!_interacting)
            StartInteraction();
        else
            EndInteraction();
    }

    protected override void StartInteraction()
    {
        _interacting = true;
        dialogueWindow.gameObject.SetActive(true);
        dialogueWindow.ShowText(dialogueText, npcImage);
    }

    public override void EndInteraction()
    {
        _interacting = false;
        dialogueWindow.gameObject.SetActive(true);
        dialogueWindow.Close();
    }
}
