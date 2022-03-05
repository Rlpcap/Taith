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

    Npc_Isa _isa;
    public GameObject tutorialPortal;

    public override void Awake()
    {
        base.Awake();

        _isa = FindObjectOfType<Npc_Isa>();
    }
    public override void Start()
    {
        base.Start();
    }

    protected override void StartInteraction()
    {
        base.StartInteraction();
        if (chatState == ChatState.Talking)
            SoundManager.PlaySound(SoundManager.Sound.MiaChatVoice);
    }

    public void StaffQuest()
    {
        if (_isa.helped)
            _questType = "MiasHatQuest";

        CheckStatus();
    }

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
