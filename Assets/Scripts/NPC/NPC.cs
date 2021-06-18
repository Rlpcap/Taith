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

    public Quest npcQuest;


    void Start()
    {
        if(npcQuest.toggleQuest)
        if(!QuestManager.Instance._listOfQuests.Contains(npcQuest))
            QuestManager.Instance.AddQuestToList(npcQuest);
    }

    public override void Interact()
    {
        if (!_interacting)
            StartInteraction();
        else
            EndInteraction();
    }

    protected override void StartInteraction()
    {
        Debug.Log("dialogueeee");

        if(dialogueWindow.isChatting)
        {
            dialogueWindow.AutoCompleteText();

            _interacting = true;

        }else
        {
            dialogueWindow.gameObject.SetActive(true);

            CheckQuest();
        }




       /* if(Quest!="")
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
        }*/

    }

    void CheckQuest()
    {
        if(!npcQuest.toggleQuest)
        {
            dialogueWindow.ShowText(dialogueText,npcImage);
            return;
        }

        if(QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName,QuestState.State.Locked))
        {
             QuestManager.Instance.ChangeQuestStatus(npcQuest.QuestName, QuestState.State.Unlocked);
        }

        if(QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName,QuestState.State.Completed) && QuestManager.Instance.GiveReward(npcQuest.questReward))
        {
            dialogueWindow.ShowText(rewardText,npcImage);

        }else
        {
            dialogueWindow.ShowText(dialogueText, npcImage);

        }

    }

    public override void EndInteraction()
    {
        _interacting = false;
        dialogueWindow.gameObject.SetActive(true);
        dialogueWindow.Close();
    }
}
