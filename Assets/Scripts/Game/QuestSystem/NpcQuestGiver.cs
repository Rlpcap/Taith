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
    public override void NPCAction()
    {

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

        }
        else
        {

        }

    }

    void AssignedQuest()
    {
        assignedQuest = true;
        quest = (QuestGiver)quests.AddComponent(System.Type.GetType(_questType));
    }

    void CheckTheQuest()
    {
        if (quest.completed)
        {
            quest.GiveReward();
            helped = true;
            assignedQuest = false;

        }else
        {
            
        }
    }
}
