using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class NPC_NoQuest : NPC
{
    public SoundManager.Sound voiceSound;
    public bool coinCounter = false;
    public int maxCoins;

    public override void Start()
    {
        base.Start();
        if (coinCounter && InventoryController.Instance.playerItems.Any(i => i.itemName == "Coin"))
        {
            defaultText = "Congratulations! You found " + InventoryController.Instance.playerItems.Where(i => i.itemName == "Coin").First().ammount + " out of " + maxCoins + " Coins!";
        }
    }

    public override void NPCAction()
    {
    }

    protected override void StartInteraction()
    {
        NPCAction();

        if (coinCounter && InventoryController.Instance.playerItems.Any(i => i.itemName == "Coin"))
        {
            defaultText = "Congratulations! You found " + InventoryController.Instance.playerItems.Where(i => i.itemName == "Coin").First().ammount + " out of " + maxCoins + " Coins!";
        }

        if (chatState == ChatState.Talking)
        {
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
}
