using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryLevelManager : MonoBehaviour
{
    void Start()
    {
        MusicManager.Instance.SwitchMusic(0);
        GameManager.Instance.gameCompleted = true;
    }
}
