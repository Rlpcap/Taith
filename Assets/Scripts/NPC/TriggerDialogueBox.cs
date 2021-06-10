using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerDialogueBox : MonoBehaviour
{
    [TextArea]
    public string dialogueText;

    public DialogueWindow dialogueWindow;

    public Sprite npcImage;


    private void OnTriggerEnter(Collider other)
    {
        var pl = other.GetComponent<PlayerModel>();

        if(pl)
        {
            dialogueWindow.gameObject.SetActive(true);
            dialogueWindow.ShowText(dialogueText,npcImage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var pl = other.GetComponent<PlayerModel>();

        if (pl)
        {
            dialogueWindow.gameObject.SetActive(true);
            dialogueWindow.Close();
        }
    }
}
