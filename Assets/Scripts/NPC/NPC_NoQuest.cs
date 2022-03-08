using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_NoQuest : NPC
{
    public SoundManager.Sound voiceSound;

    public override void NPCAction()
    {
    }

    protected override void StartInteraction()
    {
        NPCAction();

        if (chatState == ChatState.Talking)
        {
            dialogueWindow.AutoCompleteText();
            chatState = ChatState.NoTalking;
        }

        if (chatState == ChatState.StoppedTalking)
        {
            dialogueWindow.gameObject.SetActive(true);

            dialogueWindow.ShowText(defaultText, npcImage, this, npcName);

            chatState = ChatState.Talking;
        }

        if (chatState == ChatState.Talking)
            SoundManager.PlaySound(voiceSound);
    }
}
