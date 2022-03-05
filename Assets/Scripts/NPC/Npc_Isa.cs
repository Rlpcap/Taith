using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Isa : NpcQuestGiver
{
    Npc_Mia _mia;

    public override void Awake()
    {
        base.Awake();

        _mia = FindObjectOfType<Npc_Mia>();
    }

    public override void Start()
    {
        base.Start();

        if (interactedWith)
            _mia.StaffQuest();
    }
    protected override void StartInteraction()
    {
        base.StartInteraction();
        if (chatState == ChatState.Talking)
            SoundManager.PlaySound(SoundManager.Sound.IsaChatVoice);

    }

    public void ScrollQuest()
    {
        if (!helped)
            _questType = "ScrollQuest";

        CheckStatus();
    }
}
