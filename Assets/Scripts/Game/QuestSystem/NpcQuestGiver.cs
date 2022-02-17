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

    public QuestGiver quest;

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
        if (!assignedQuest && !helped)
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
        }


    }

    void AssignedQuest()
    {
        if (_questType != null)
        {
            assignedQuest = true;
            quest = (QuestGiver)quests.AddComponent(System.Type.GetType(_questType));
        }

        dialogueWindow.ShowText(dialogueText, npcImage, this);

    }

    public virtual void CheckTheQuest()
    {
        if (quest.completed)
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

        }
    }
}
