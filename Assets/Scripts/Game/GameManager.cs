﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

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
    public bool canUseQuestsUI = false;
    SoundSpawner _soundSpawner;

    // una lista de clips para probar, despues cambio esto. No tocar por favor!

    public SoundAudioClip[] soundClips;

    public bool hasToPlayCinematic;

    public Quest quest;

    public GameObject tutorialPortal;

    public QuestGiver newQuest;
    public GameObject quests;

    public List<int> metNPCs = new List<int>();
    List<int> metEnemies = new List<int>();
    List<int> completedTutorialPortals = new List<int>();
    public bool completedTutorial = false;
    public int endPortalsCount;
    public List<int> completedEndPortals = new List<int>();
    public bool bossTime = false;
    public bool gameCompleted = false;

    PlayerModel _playerModel;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundManager.Sound sound;
        public AudioClip audioClip;
    }

    protected override void Awake()
    {
        base.Awake();

        _playerModel = FindObjectOfType<PlayerModel>();
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

        _playerModel.playerView.scrollsSlot.text = "" + 0;
        _playerModel.playerView.bouquetsSlot.text = "" + 0;
        _playerModel.playerView.coinsSlot.text = "" + 0;
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

    public void MeetNPC(int index)
    {
        if (!metNPCs.Contains(index))
            metNPCs.Add(index);
    }

    public bool FirstTimeEnemy(int id)
    {
        if (!metEnemies.Contains(id))
        {
            metEnemies.Add(id);
            return true;
        }
        else
            return false;
    }

    public void CompleteTutorialPortal(int id)
    {
        if (!completedTutorialPortals.Contains(id))
        {
            completedTutorialPortals.Add(id);
            if(lastLevelAchieved < maxLevel + 1)
            {
                lastLevelAchieved++;
                hasToPlayCinematic = true;
            }
        }
    }

    public void CompleteEndPortal(int id)
    {
        if (!completedEndPortals.Contains(id))
            completedEndPortals.Add(id);
        if (completedEndPortals.Count >= endPortalsCount)
        {
            hasToPlayCinematic = true;
            bossTime = true;
        }
    }
}
