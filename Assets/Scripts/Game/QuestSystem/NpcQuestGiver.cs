using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NpcQuestGiver : NPC
{
    public bool assignedQuest;
    public bool helped;

    [SerializeField]
    private GameObject quests;

    [SerializeField]
    private string _questType;

    protected QuestGiver _quest;

    public bool interactedWith;


    public override void Awake()
    {
        base.Awake();

        quests = FindObjectOfType<QuestManager>().gameObject;
        /* if (quests.GetComponent(_questType))
         {
             _quest = (QuestGiver)quests.GetComponent(_questType);
             interactedWith = true;
             assignedQuest = !_quest.completed;
             Debug.Log(_quest.completed);
             helped = _quest.completed && _quest.gaveReward;
         }*/
    }

    public override void Start()
    {
        base.Start();
        if (quests.GetComponent(_questType))
        {
            _quest = (QuestGiver)quests.GetComponent(_questType);

            interactedWith = true;
            assignedQuest = !_quest.completed;

            helped = _quest.completed && _quest.gaveReward;


        }
    }

    public override void NPCAction()
    {
        if (!interactedWith)
        {
            InventoryController.Instance.GiveItem("TalkedTo" + npcName);
            InventoryController.Instance.GiveItem("VillagersTalked");
            interactedWith = true;
        }
    }

    public override void Interact()
    {
        base.Interact();
        /* if (!assignedQuest && !helped)
         {
             AssignedQuest();
         }
         else if (assignedQuest && !helped)
         {
             CheckTheQuest();
         }
         else
         {
             //repetir dialogo de recompensa
             dialogueWindow.ShowText(rewardText, npcImage, this);
         }*/

    }

    protected override void StartInteraction()
    {
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

            CheckTheQuest();
            chatState = ChatState.Talking;
        }

    }


    public virtual void CheckTheQuest()
    {
        /* if (quest.completed)
         {
             quest.GiveReward();
             helped = true;
             assignedQuest = false;
             //llamo dialogo de recompensa en el npc.
             if (chatState == ChatState.Talking && _interacting)
             {
                 dialogueWindow.ShowText(rewardText, npcImage, this);
             }

         }
         else
         {
             //llamo el dialogo de siempre
             if (chatState == ChatState.Talking && _interacting)
             {
                 dialogueWindow.ShowText(dialogueText, npcImage, this);
             }

         }*/

        if (_questType == "")
        {
            dialogueWindow.ShowText(dialogueText, npcImage, this, npcName);
            return;
        }

        if (!assignedQuest && !helped)
        {
            AssignedQuest();
        }

        if (assignedQuest && _quest != null)
            _quest.CheckInteractionNPC("TalkedTo" + npcName);

        if (_quest.completed && !helped)
        {
            _quest.GiveReward();
            _quest.gaveReward = true;
            helped = true;
            assignedQuest = false;
            //llamo dialogo de recompensa en el npc.
            dialogueWindow.ShowText(rewardText, npcImage, this, npcName);

        }
        else
        {
            dialogueWindow.ShowText(dialogueText, npcImage, this, npcName);

        }

    }

    public override void ShowMarks()
    {
        if (_questType != "")
        {
            if (!assignedQuest || (_quest != null && _quest.CheckSteps()))
            {
                _anim.SetBool("quest", true);
            }
            else
                _anim.SetBool("quest", false);

            if (!assignedQuest)
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


            if (_quest != null && _quest.CheckSteps())
            {
                questMark.SetActive(true);

                if (_npcParticleSign != null)
                    _npcParticleSign.gameObject.SetActive(true);

            }
            else
            {
                questMark.SetActive(false);
            }

            if (helped)
            {
                questMark.SetActive(false);
                exclamationMark.SetActive(false);
                _anim.SetBool("quest", false);
            }

        }
        else
        {
            exclamationMark.SetActive(false);
            questMark.SetActive(false);
            _anim.SetBool("quest", false);

            if (_npcParticleSign != null && _npcParticleSign.gameObject.activeInHierarchy)
                _npcParticleSign.gameObject.SetActive(false);
        }
    }


    void AssignedQuest()
    {
        if (_questType != null && _quest == null)
        {
            assignedQuest = true;
            _quest = (QuestGiver)quests.AddComponent(System.Type.GetType(_questType));
            QuestManager.Instance.quests.Add(_quest);
            StartCoroutine(QuestManager.Instance.UpdateQuestUI());
        }

        // dialogueWindow.ShowText(dialogueText, npcImage, this);

    }

}