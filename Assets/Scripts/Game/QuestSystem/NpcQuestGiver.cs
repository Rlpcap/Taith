using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcQuestGiver : NPC
{
    public bool assignedQuest;
    public bool helped;

    [SerializeField]
    private GameObject quests;

    [SerializeField]
    private string _questType;

    protected QuestGiver _quest;

    private bool _interactedWith;

    public override void NPCAction()
    {
        if (!_interactedWith)
        {
            InventoryController.Instance.GiveItem("VillagerTalked");
            _interactedWith = true;
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
            dialogueWindow.ShowText(dialogueText, npcImage, this);
            return;
        }

        if (!assignedQuest && !helped)
        {
            AssignedQuest();
        }

        if (_quest.completed && !helped)
        {
            _quest.GiveReward();
            helped = true;
            assignedQuest = false;
            //llamo dialogo de recompensa en el npc.
            dialogueWindow.ShowText(rewardText, npcImage, this);

        }
        else
        {
            dialogueWindow.ShowText(dialogueText, npcImage, this);

        }

    }

    public override void ShowMarks()
    {
        if (_questType != "")
        {

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


            if (_quest != null && _quest.completed)
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
            }

        }
        else
        {
            exclamationMark.SetActive(false);
            questMark.SetActive(false);

            if (_npcParticleSign != null && _npcParticleSign.gameObject.activeInHierarchy)
                _npcParticleSign.gameObject.SetActive(false);
        }
    }


    void AssignedQuest()
    {
        if (_questType != null)
        {
            assignedQuest = true;
            _quest = (QuestGiver)quests.AddComponent(System.Type.GetType(_questType));
            QuestManager.Instance.quests.Add(_quest);
        }

        // dialogueWindow.ShowText(dialogueText, npcImage, this);

    }

}
