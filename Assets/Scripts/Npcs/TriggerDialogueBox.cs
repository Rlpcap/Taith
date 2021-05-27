using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueBox : MonoBehaviour
{
    [TextArea]
    public string dialogueText;

    public DialogueWindow dialogueWindow;


    private void OnTriggerEnter(Collider other)
    {
        var pl = other.GetComponent<PlayerModel>();

        if(pl && dialogueWindow.gameObject.activeInHierarchy)
        {
            dialogueWindow.ShowText(dialogueText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var pl = other.GetComponent<PlayerModel>();

        if (pl && dialogueWindow.gameObject.activeInHierarchy)
        {
            dialogueWindow.Close();
        }
    }
}
