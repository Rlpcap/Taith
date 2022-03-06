using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitDummyQuest : QuestGiver
{
    LobbyManager _lobbyManager;

    public override void Awake()
    {
        base.Awake();

        _lobbyManager = FindObjectOfType<LobbyManager>();
    }
    public override void Start()
    {

        questName = "Trainning for dummies";
        questDescription = "Practice attacking with the dummies";
        goals = new List<QuestGoal>();

        goals.Add(new CollectionGoal(this, "HitDummy", "Hit the dummy twice", false, 0, 2, TypeOfGoal.Item));
        goals.Add(new CollectionGoal(this, "TalkedToRen", "Talk to Ren", false, 0, 2, TypeOfGoal.Item));

        foreach (QuestGoal g in goals)
        {
            Debug.Log(g.description);
            g.Init();
        }
        Debug.Log("Quest added!");
        base.Start();

        pv.ToggleQuestsUI();
    }

    public override void CallRewardEvent()
    {
        _lobbyManager.tutorialPlatform.SetActive(true);
    }

    public HitDummyQuest SetLobbyManager(LobbyManager lm)
    {
        _lobbyManager = lm;
        return this;
    }
}
