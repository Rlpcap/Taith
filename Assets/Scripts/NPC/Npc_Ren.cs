using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Ren : NpcQuestGiver
{
    protected override void StartInteraction()
    {
        base.StartInteraction();
        if (chatState == ChatState.Talking)
            SoundManager.PlaySound(SoundManager.Sound.RenChatVoice);
    }
}
