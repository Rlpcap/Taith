using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Ren : NPC
{

    public override void NPCAction()
    {
        questMark.SetActive(false);
        QuestManager.Instance.CheckTask("Talk to all the villagers", "Talk to Ren", true);
    }
}
