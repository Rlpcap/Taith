using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Mia : NPC
{
    
    public override void NPCAction()
    {
        questMark.SetActive(false);
        QuestManager.Instance.CheckTask("Talk to all the villagers", "Talk to Mia", true);

        if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Completed))
            GameManager.Instance.canUseBook = true;
    }
}
