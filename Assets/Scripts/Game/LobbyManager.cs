using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public PlayerModel playerModel;
    public CameraTarget cameraTarget;
    public List<Transform> spawnPositions = new List<Transform>();
    public List<GameObject> lockedIslands = new List<GameObject>();
    public List<GameObject> corruptedIslands = new List<GameObject>();

    void Start()
    {
        playerModel.transform.position = spawnPositions[GameManager.Instance.lobbySpawnIndex].position;
        playerModel.transform.rotation = spawnPositions[GameManager.Instance.lobbySpawnIndex].rotation;
        cameraTarget.transform.position = spawnPositions[GameManager.Instance.lobbySpawnIndex].position;
        cameraTarget.transform.rotation = spawnPositions[GameManager.Instance.lobbySpawnIndex].rotation;

        for (int i = GameManager.Instance.maxLevel-1; i > GameManager.Instance.lastLevelAchieved-1; i--)
        {
            lockedIslands[i].SetActive(false);
            corruptedIslands[i].SetActive(true);
        }
    }
}
