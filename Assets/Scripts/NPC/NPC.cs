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
        {
            _interacting = true;
            StartInteraction();
        }
        else
        {
            _interacting = false;
            EndInteraction();
        }
    }

    protected override void StartInteraction()
    {
        dialogueWindow.gameObject.SetActive(true);
        dialogueWindow.ShowText(dialogueText, npcImage);
    }

    protected override void EndInteraction()
    {
        dialogueWindow.gameObject.SetActive(true);
        dialogueWindow.Close();
    }
}
