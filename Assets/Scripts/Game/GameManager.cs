﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int maxLevel = 5;
    public int lastLevelAchieved = 0;

    public int lobbySpawnIndex = 0;

    SoundSpawner _soundSpawner;

// una lista de clips para probar, despues cambio esto. No tocar por favor!

        public SoundAudioClip[] soundClips;

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
        var obj = Instantiate(Resources.Load<SoundSpawner>("prefabs/SoundSpawner"));
    }
}
