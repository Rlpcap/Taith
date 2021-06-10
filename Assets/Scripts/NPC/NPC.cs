﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable
{
    [TextArea]
    public string dialogueText;

    [TextArea]
    public string rewardText;

    public DialogueWindow dialogueWindow;

    public Sprite npcImage;

    public string Quest;

  //  public Quest npcQuest;

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
        
        if(Quest!="")
        {
            if(QuestManager.Instance.CheckQuestStatus(Quest,QuestState.State.Locked))
            {
                QuestManager.Instance.ChangeQuestStatus(Quest, QuestState.State.Unlocked);
            }

            if(!QuestManager.Instance.CheckQuestStatus(Quest,QuestState.State.Completed))
            {
                dialogueWindow.ShowText(dialogueText, npcImage);

            }else
            {
                dialogueWindow.ShowText(rewardText,npcImage);
            }
        }else
        {
            dialogueWindow.ShowText(dialogueText,npcImage);
        }

    }

    public override void EndInteraction()
    {
        _interacting = false;
        dialogueWindow.gameObject.SetActive(true);
        dialogueWindow.Close();
    }
}
