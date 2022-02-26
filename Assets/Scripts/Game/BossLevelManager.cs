using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BossLevelManager : MonoBehaviour
{
    public PlayerModel playerModel;
    public CameraTarget cameraTarget;
    public List<Transform> spawnPositions = new List<Transform>();
    public List<GameObject> allIslands = new List<GameObject>();
    public List<GameObject> allChallenges = new List<GameObject>();

    private void Awake()
    {
        PlaceCharacter();

        foreach (var islands in allIslands.Take(GameManager.Instance.BossLevelIndex))
            islands.SetActive(false);

        if (GameManager.Instance.BossLevelIndex > 0 && GameManager.Instance.BossLevelIndex < allChallenges.Count + 1)
            allChallenges[GameManager.Instance.BossLevelIndex - 1].SetActive(true);
    }

    void Start()
    {
        GameManager.Instance.OnVariableChange = UpdateLevel;
    }

    void PlaceCharacter()
    {
        if(GameManager.Instance.BossLevelIndex < spawnPositions.Count)
        {
            playerModel.transform.position = spawnPositions[GameManager.Instance.BossLevelIndex].position;
            playerModel.transform.rotation = spawnPositions[GameManager.Instance.BossLevelIndex].rotation;
            cameraTarget.transform.position = spawnPositions[GameManager.Instance.BossLevelIndex].position;
            cameraTarget.transform.rotation = spawnPositions[GameManager.Instance.BossLevelIndex].rotation;
        }
    }

    void UpdateLevel(int currIndex)
    {
        if (currIndex > 1)
            allChallenges[currIndex - 2].SetActive(false);
        if (currIndex - 1 < allChallenges.Count)
            allChallenges[currIndex - 1].SetActive(true);
    }
}
