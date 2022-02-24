using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : Interactable, IUpdate
{
    public string npcName;

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

    protected ParticleSystem _npcParticleSign;

    public virtual void Awake()
    {
        name = this.gameObject.name;
        _pv = FindObjectOfType<PlayerView>();
        _npcParticleSign = GetComponentInChildren<ParticleSystem>();

    }


    public virtual void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);

        chatState = ChatState.StoppedTalking;
        //CheckQuestList();

        /*if(!QuestManager.Instance._listOfQuests.Contains(npcQuest))
            QuestManager.Instance.AddQuestToList(npcQuest);*/

    }

    public virtual void ShowMarks()
    {
      /*  if (npcQuest.toggleQuest)
        {

            if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Locked))
            {
                exclamationMark.SetActive(true);
                if (_npcParticleSign != null && !_npcParticleSign.gameObject.activeInHierarchy)
                    _npcParticleSign.gameObject.SetActive(true);
            }
            else
            {
                exclamationMark.SetActive(false);
                if (_npcParticleSign != null && _npcParticleSign.gameObject.activeInHierarchy)
                    _npcParticleSign.gameObject.SetActive(false);
            }


            if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Completed))
            {
                questMark.SetActive(true);
                if (_npcParticleSign != null)
                    _npcParticleSign.gameObject.SetActive(true);

            }
            else
            {
                questMark.SetActive(false);
            }

        }
        else
        {
            exclamationMark.SetActive(false);
            questMark.SetActive(false);

            if (_npcParticleSign != null && _npcParticleSign.gameObject.activeInHierarchy)
                _npcParticleSign.gameObject.SetActive(false);
        }*/

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
        ShowMarks();
    }


    public override void Interact()
    {
        if (!_interacting)
            StartInteraction();
        else
            EndInteraction();
    }

   protected override void StartInteraction()
    {/*
        NPCAction();

        //_interacting = true;


        if (chatState == ChatState.Talking)
        {
            Debug.Log("Interrupt NPC!!");
            dialogueWindow.AutoCompleteText();
            chatState = ChatState.NoTalking;

        }

        if (chatState == ChatState.StoppedTalking)
        {
            dialogueWindow.gameObject.SetActive(true);

            // CheckQuest();
            chatState = ChatState.Talking;
        }
*/
    }

    protected virtual void CheckQuest()
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
