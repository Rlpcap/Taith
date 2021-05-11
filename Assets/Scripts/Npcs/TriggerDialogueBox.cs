using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueBox : MonoBehaviour
{
    [TextArea]
    public string dialogueText;

    public DialogueWindow dialogueWindow;

    int _player = 8;


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == _player)
        {
            Debug.Log("TRIGGER TEXT!");
            dialogueWindow.ShowText(dialogueText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer==_player)
        {
            dialogueWindow.Close();
        }
    }
}
