﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{

    public string QuestName;
    public QuestState.State QuestStatus;
    public string[] tasks;
    public Dictionary<string,bool> tasksList = new Dictionary<string, bool>();


}
