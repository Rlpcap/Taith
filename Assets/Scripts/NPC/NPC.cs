using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : Interactable, IUpdate
{
    [TextArea]
    public string dialogueText;

    [TextArea]
    public string rewardText;

    public DialogueWindow dialogueWindow;

    public Sprite npcImage;

    public Quest npcQuest;

    public ChatState chatState;



    void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);

        chatState = ChatState.StoppedTalking;
        CheckQuestList();

        /*if(!QuestManager.Instance._listOfQuests.Contains(npcQuest))
            QuestManager.Instance.AddQuestToList(npcQuest);*/

    }

    void CheckQuestList()
    {
        var list = QuestManager.Instance._listOfQuests;
        if (npcQuest.toggleQuest)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].QuestName == npcQuest.QuestName)
                {
                    return;
                }
            }
            QuestManager.Instance.AddQuestToList(npcQuest);
        }

    }

    public void OnUpdate()
    {
        switch (chatState)
        {
            case ChatState.Talking:
                {
                    _interacting = false;
                }
                break;
            case ChatState.NoTalking:
                {
                    _interacting = true;
                }
                break;
            case ChatState.StoppedTalking:
                {
                    _interacting = false;
                }
                break;
        }
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

        if (chatState == ChatState.Talking)
        {
            dialogueWindow.AutoCompleteText();
            chatState = ChatState.NoTalking;

        }

        if (chatState == ChatState.StoppedTalking)
        {
            dialogueWindow.gameObject.SetActive(true);

            CheckQuest();
            chatState = ChatState.Talking;
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
        if (!npcQuest.toggleQuest)
        {
            dialogueWindow.ShowText(dialogueText, npcImage, this);
            return;
        }

        if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Locked))
        {
            QuestManager.Instance.ChangeQuestStatus(npcQuest.QuestName, QuestState.State.Unlocked);
        }

        if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Completed) && QuestManager.Instance.GiveReward(npcQuest.questReward))
        {
            dialogueWindow.ShowText(rewardText, npcImage, this);

        }
        else
        {
            dialogueWindow.ShowText(dialogueText, npcImage, this);

        }

    }

    public override void EndInteraction()
    {
        _interacting = false;
        chatState = ChatState.StoppedTalking;
        dialogueWindow.gameObject.SetActive(true);
        dialogueWindow.Close();
    }


    public enum ChatState
    {
        Talking,
        StoppedTalking,
        NoTalking,
    }
}
