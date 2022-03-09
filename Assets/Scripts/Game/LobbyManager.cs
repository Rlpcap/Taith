using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    public PlayerModel playerModel;
    public CameraTarget cameraTarget;
    public List<Transform> spawnPositions = new List<Transform>();
    public List<GameObject> lockedIslands = new List<GameObject>();
    public List<GameObject> corruptedIslands = new List<GameObject>();

    public GameObject tutorialPortal;

    public GameObject levelPortals;
    public List<Portal> levelPortalsList = new List<Portal>();

    [ColorUsage(true, true)]
    public Color lobbyTransitionColor;
    public GameObject transitionScreen;

    public GameObject scroll, hat, tutorialPlatform;

    public GameObject goodElder, badElder, tutRen, coinCounterRen;

    void Awake()
    {
        if (QuestManager.Instance.quests.Count != 0)
            foreach (var q in QuestManager.Instance.quests)
            {
                q.pv = FindObjectOfType<PlayerView>();
            }

    }
    void Start()
    {
        /*UpdateData.Instance.CheckNPCSOnScene();
        QuestManager.Instance.LoadQuests();
        UpdateData.Instance.LoadNPCData();*/
        if (QuestManager.Instance.GetComponent<HitDummyQuest>())
        {
            QuestManager.Instance.GetComponent<HitDummyQuest>().SetLobbyManager(this);
        }

        transitionScreen.GetComponent<Renderer>().sharedMaterial.SetColor("_Color", lobbyTransitionColor);

        playerModel.transform.position = spawnPositions[GameManager.Instance.lobbySpawnIndex].position;
        playerModel.transform.rotation = spawnPositions[GameManager.Instance.lobbySpawnIndex].rotation;
        cameraTarget.transform.position = spawnPositions[GameManager.Instance.lobbySpawnIndex].position;
        cameraTarget.transform.rotation = spawnPositions[GameManager.Instance.lobbySpawnIndex].rotation;

        for (int i = GameManager.Instance.maxLevel - 1; i > GameManager.Instance.lastLevelAchieved - 1; i--)
        {
            lockedIslands[i].SetActive(false);
            corruptedIslands[i].SetActive(true);
        }

        CheckCompletedTutorial();

        /*  if (QuestManager.Instance.CheckQuestStatus("The Scroll Quest", QuestState.State.Completed))
              tutorialPortal.SetActive(true);*/

        var miasHatQuest = GameObject.FindObjectOfType<MiasHatQuest>();

        if (miasHatQuest != null && miasHatQuest.completed)
        {
            tutorialPortal.SetActive(true);
        }

        var VillagerTalked = FindObjectOfType<TalkToAllVillagersQuest>();

        if (VillagerTalked != null)
        {
            playerModel.playerView.ToggleQuestsUI();
        }

        var dummyQuest = FindObjectOfType<HitDummyQuest>();

        if (dummyQuest != null && dummyQuest.completed)
        {
            tutorialPlatform.SetActive(true);
        }
        else
        {
            tutorialPlatform.SetActive(false);
        }

        if (GameManager.Instance.gameCompleted)
        {
            goodElder.SetActive(false);
            badElder.SetActive(true);

            tutRen.SetActive(false);
            coinCounterRen.SetActive(true);
        }
    }

    void CheckCompletedTutorial()
    {
        if (GameManager.Instance.completedTutorial)
        {
            levelPortals.SetActive(true);
            foreach (var portal in levelPortalsList.Where(x => GameManager.Instance.completedEndPortals.Any(p => p == x.ID)))
            {
                portal.gameObject.SetActive(false);
            }
        }
    }
}
