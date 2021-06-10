using System.Collections;
using System.Collections.Generic;

public class Quest
{
    public string QuestName;
    public QuestState.State QuestStatus;

    public Dictionary<string,bool> tasks = new Dictionary<string, bool>();


}
