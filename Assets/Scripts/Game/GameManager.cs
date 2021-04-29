using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int maxLevel = 5;
    public int lastLevelAchieved = 0;

    public int lobbySpawnIndex = 0;

    void Start()
    {

    }
}
