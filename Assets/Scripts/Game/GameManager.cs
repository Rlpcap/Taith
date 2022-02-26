using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int maxLevel = 5;
    public int lastLevelAchieved = 0;

    public int lobbySpawnIndex = 0;

    [SerializeField]
    int _bossLevelIndex = 0;
    public int BossLevelIndex 
    { 
        get 
        { return _bossLevelIndex; } 
        set 
        { 
            _bossLevelIndex = value;
            OnVariableChange(value);
        }
    }
    public Action<int> OnVariableChange = delegate { };

    public string loadingLevel = "LevelIntro";

    public List<int> inventoryList = new List<int>();

    public bool canUseBook = false;
    SoundSpawner _soundSpawner;

    // una lista de clips para probar, despues cambio esto. No tocar por favor!

    public SoundAudioClip[] soundClips;

    public bool hasToPlayCinematic;

    public Quest quest;

    public GameObject tutorialPortal;

    public QuestGiver newQuest;
    public GameObject quests;


    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    void Start()
    {
        SoundManager.soundTimer = new Dictionary<SoundManager.Sound, float>();
        SoundManager.soundTimer[SoundManager.Sound.PlatformShake] = 0f;
        SoundManager.soundTimer[SoundManager.Sound.MudStep] = 0f;

        QuestManager.Instance.AddQuestToList(quest);
        QuestManager.Instance.ChangeQuestStatus(quest.QuestName, QuestState.State.Unlocked);
        Debug.Log(QuestManager.Instance._listOfQuests.Count);
        EventListener.OnEvent += ShowTutorialPortal;

       // newQuest = (QuestGiver)quests.AddComponent(System.Type.GetType("TalkToAllVillagersQuest"));

    }

    public void HideObjects(PickupObject[] objectsInScene)
    {
      /*  foreach (var o in objectsInScene)
        {
            if (inventoryList.Contains(o.id))
                o.gameObject.SetActive(false);
        }*/
    }

    public void ShowTutorialPortal()
    {
        if (tutorialPortal != null && !tutorialPortal.activeInHierarchy)
            tutorialPortal.SetActive(true);
    }

}
