using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Elder : NpcQuestGiver
{
    [TextArea]
    public string slaveExplainText;

    public GameObject finalSlaves;

    public Transform tutorialSpot;

    Npc_Isa _isa;

    public override void Awake()
    {
        base.Awake();

        _isa = FindObjectOfType<Npc_Isa>();
    }

    public override void Start()
    {
        base.Start();

        if (GameManager.Instance.lastLevelAchieved == 1)
        {
            transform.position = tutorialSpot.position;
            transform.rotation = tutorialSpot.rotation;
        }

        if(interactedWith)
            _isa.ScrollQuest();
    }

    public override void CheckTheQuest()
    {
        base.CheckTheQuest();

        if (!_quest.completed)
        {
            if (GameManager.Instance.lastLevelAchieved == 1)
                dialogueWindow.ShowText(slaveExplainText, npcImage, this, npcName);
            else
                dialogueWindow.ShowText(dialogueText, npcImage, this, npcName);

        }
    }

    public void ShowEndPortals()
    {
        GameManager.Instance.completedTutorial = true;
        finalSlaves.SetActive(true);
    }

    protected override void StartInteraction()
    {
        base.StartInteraction();
        if (chatState == ChatState.Talking)
            SoundManager.PlaySound(SoundManager.Sound.ElderChatVoice);

        _isa.ScrollQuest();
    }


    /* public override void NPCAction()
     {
         questMark.SetActive(false);
         QuestManager.Instance.CheckTask("Talk to all the villagers", "Talk to the elder", true);

         if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Completed))
             finalSlaves.SetActive(true);
     }
      protected override void CheckQuest()
     {
         if (!npcQuest.toggleQuest)
         {
             dialogueWindow.ShowText(dialogueText, npcImage, this);
             return;
         }

         if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Locked))
         {
             QuestManager.Instance.ChangeQuestStatus(npcQuest.QuestName, QuestState.State.Unlocked);
         }

         if (QuestManager.Instance.CheckQuestStatus(npcQuest.QuestName, QuestState.State.Completed) && QuestManager.Instance.GiveReward(npcQuest.questReward))
         {
             dialogueWindow.ShowText(rewardText, npcImage, this);
             npcQuest.toggleQuest = false;

         }
         else
         {
             if (GameManager.Instance.lastLevelAchieved == 1)
                 dialogueWindow.ShowText(slaveExplainText, npcImage, this);
             else
                 dialogueWindow.ShowText(dialogueText, npcImage, this);
         }
     }*/
}
