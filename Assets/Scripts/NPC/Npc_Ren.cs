using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Npc_Ren : NpcQuestGiver
{
    public CinematicPlayer cp;
    public PlayableAsset cinematic;

    protected override void StartInteraction()
    {
        base.StartInteraction();
        if (chatState == ChatState.Talking)
            SoundManager.PlaySound(SoundManager.Sound.RenChatVoice);
    }

    public void CompleteQuest()
    {
        cp.PlayCutscene(cinematic);
    }
}
