using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Dev : NPC
{
    public SoundManager.Sound voiceSound;

    public override void NPCAction()
    {

    }

    protected override void StartInteraction()
    {
        NPCAction();

        //_interacting = true;

        if (chatState == ChatState.Talking)
        {
            Debug.Log("Interrupt NPC!!");
            dialogueWindow.AutoCompleteText();
            chatState = ChatState.NoTalking;

        }

        if (chatState == ChatState.StoppedTalking)
        {
            dialogueWindow.gameObject.SetActive(true);

            dialogueWindow.ShowText(defaultText, npcImage, this, npcName);

            chatState = ChatState.Talking;
        }

        if (chatState == ChatState.Talking)
            SoundManager.PlaySound(voiceSound);
    }

    void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.layer == 8)
        {
            var random = Random.Range(1, 4);
            switch (random)
            {
                case 1:
                    coll.transform.parent.GetComponentInParent<PlayerModel>().SetOnFire(1);
                    break;
                case 2:
                    coll.transform.parent.GetComponentInParent<PlayerModel>().CallFreeze(1);
                    break;
                default:
                    coll.transform.parent.GetComponentInParent<PlayerModel>().CallStopInTime(1);
                    break;
            }
        }
    }
}
