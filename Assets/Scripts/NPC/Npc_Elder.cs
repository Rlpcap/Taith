using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Elder : NPC
{
    public override void NPCAction()
    {
        questMark.SetActive(false);
        QuestManager.Instance.CheckTask("Talk to all the villagers", "Talk to the elder", true);
    }
}
