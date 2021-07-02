using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : Interactable, IUpdate
{
    [TextArea]
    public string dialogueText;

    [TextArea]
    public string rewardText;

    public DialogueWindow dialogueWindow;

    public Sprite npcImage;

    public Quest npcQuest;

    public ChatState chatState;

    public GameObject questMark, exclamationMark;

    protected PlayerView _pv;

    void Awake()
    {
        _pv = FindObjectOfType<PlayerView>();
    }


    public virtual void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);

        chatState = ChatState.StoppedTalking;
        CheckQuestList();

        /*if(!QuestManager.Instance._listOfQuests.Contains(npcQuest))
            QuestManager.Instance.AddQuestToList(npcQuest);*/

    }

    public void showMarks()
    {
        if (npcQuest.toggleQuest)
        {
            if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Locked))
                exclamationMark.SetActive(true);
            else
                exclamationMark.SetActive(false);


            if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Completed))
                questMark.SetActive(true);
            else
                questMark.SetActive(false);

        }
        else
        {
            exclamationMark.SetActive(false);
            questMark.SetActive(false);
        }

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
        showMarks();
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

        NPCAction();


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
            npcQuest.toggleQuest = false;

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


    public abstract void NPCAction();

    public enum ChatState
    {
        Talking,
        StoppedTalking,
        NoTalking,
    }
}
