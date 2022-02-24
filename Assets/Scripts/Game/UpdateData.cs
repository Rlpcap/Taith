using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class UpdateData : Singleton<UpdateData>
{

    List<NPC_Info> npcsInfo = new List<NPC_Info>();

    List<NpcQuestGiver> npcs;
    protected override void Awake()
    {
        base.Awake();
        CheckNPCSOnScene();
    }

    public void CheckNPCSOnScene()
    {
        npcs = FindObjectsOfType<NpcQuestGiver>().ToList();
    }

    public void SaveNPCData()
    {
        if (npcsInfo.Count != npcs.Count)
        {
            npcsInfo = new List<NPC_Info>(npcs.ToList().Count - npcsInfo.Count);
        }
        foreach (var npc in npcs)
        {
            foreach (var i in npcsInfo)
            {

                if (i.npcName != "")
                {
                    i.npcName = npc.npcName;
                    i.assignedQuest = npc.assignedQuest;
                    i.helped = npc.helped;
                    i.interactedWith = npc.interactedWith;
                }
                else if (npc.npcName == i.npcName)
                {
                    i.npcName = npc.npcName;
                    i.assignedQuest = npc.assignedQuest;
                    i.helped = npc.helped;
                    i.interactedWith = npc.interactedWith;
                }
            }
        }
    }

    public void LoadNPCData()
    {
        if (npcs.Count != 0)
        {
            foreach (var npc in npcs)
            {
                foreach (var i in npcsInfo)
                {
                    i.npcName = npc.npcName;
                    i.assignedQuest = npc.assignedQuest;
                    i.helped = npc.helped;
                    i.interactedWith = npc.interactedWith;
                }
            }
        }
    }


}

public class NPC_Info : NpcQuestGiver
{

}
