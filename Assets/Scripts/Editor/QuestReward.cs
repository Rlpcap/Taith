using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "QuestReward", menuName = "ScriptableObjects/SpawnQuestRewardObject",order = 1)]
public class QuestReward : ScriptableObject
{
    public RewardType rewardType;

    public float stat;

    public GameObject item;
    public enum RewardType
    {
        Stat,
        Item,
    }
}
