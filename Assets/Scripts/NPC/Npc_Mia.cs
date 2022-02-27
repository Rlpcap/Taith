using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Mia : NpcQuestGiver
{
    /*public override void Start()
    {
        base.Start();
        showMarks();
    }*/

    public GameObject tutorialPortal;

  /*  public override void NPCAction()
    {
        exclamationMark.SetActive(false);
        questMark.SetActive(false);
        QuestManager.Instance.CheckTask("Talk to all the villagers", "Talk to Mia", true);

        if (QuestManager.Instance.CurrentTask("Get the scroll and bring it back to Mia", "Get the scroll", true))
            QuestManager.Instance.CheckTask("Get the scroll and bring it back to Mia", "Bring back the scroll to mia", true);

        //if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Completed))
        //{
        //    _pv.ShowBookUI();
        //    tutorialPortal.SetActive(true);
        //}
    }*/

    public void ShowTutorialPortal()
    {
        //GameManager.Instance.completedTutorial = true;
        tutorialPortal.SetActive(true);
    }
}
