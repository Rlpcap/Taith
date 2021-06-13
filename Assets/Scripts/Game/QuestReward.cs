using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestReward
{
    public RewardType rewardType;

    [System.Serializable]
    public delegate void MyDelegate();

    public float stat;

    public bool rewardGiven = false;
    public enum RewardType
    {
        Stat,
    }
}
