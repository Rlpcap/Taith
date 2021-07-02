using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Elder : NPC
{
    public GameObject finalSlaves;

    public override void NPCAction()
    {
        questMark.SetActive(false);
        QuestManager.Instance.CheckTask("Talk to all the villagers", "Talk to the elder", true);

        if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Completed))
            finalSlaves.SetActive(true);
    }
}
