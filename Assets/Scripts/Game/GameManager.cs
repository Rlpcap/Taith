using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int maxLevel = 5;
    public int lastLevelAchieved = 0;

    public int lobbySpawnIndex = 0;

    public List<int> inventoryList = new List<int>();

    public bool canUseBook = false;
    SoundSpawner _soundSpawner;

    // una lista de clips para probar, despues cambio esto. No tocar por favor!

    public SoundAudioClip[] soundClips;

    public bool hasToPlayCinematic;

    public Quest quest;


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

        QuestManager.Instance.AddQuestToList(quest);
        QuestManager.Instance.ChangeQuestStatus(quest.QuestName, QuestState.State.Unlocked);
        Debug.Log(QuestManager.Instance._listOfQuests.Count);
    }

    public void HideObjects(PickupObject[] objectsInScene)
    {
        foreach (var o in objectsInScene)
        {
            if (inventoryList.Contains(o.id))
                o.gameObject.SetActive(false);
        }
    }



}
