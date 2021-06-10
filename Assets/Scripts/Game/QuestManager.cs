using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuestManager : Singleton<QuestManager>
{
    public List<Quest> _listOfQuests = new List<Quest>();


    void Start()
    {
        for (int i = 0; i < _listOfQuests.Count; i++)
        {
            for (int j = 0; j < _listOfQuests[i].tasks.Length; j++)
            {
                 _listOfQuests[i].tasksList.Add(_listOfQuests[i].tasks[j],false);
                
            }
        }

        foreach (var q in _listOfQuests)
        {
            q.QuestStatus = QuestState.State.Locked;
        }
    }

    void Update()
    {
        Debug.Log(_listOfQuests[0].QuestName);
        Debug.Log(""+_listOfQuests[0].tasksList[_listOfQuests[0].tasks[0]]);
        Debug.Log("Estado de la quest: "+ _listOfQuests[0].QuestStatus.ToString());
    }

    public void ChangeQuestStatus(string questName, QuestState.State newStatus)
    {
        var quest = _listOfQuests.Where(x=> x.QuestName == questName).First();
        
        quest.QuestStatus = newStatus;

        CheckQuest(quest);

    }
    public void CheckTask(string questName, string taskName, bool checker)
    {
        var quest = _listOfQuests.Where(x=>x.QuestName == questName).First();

        quest.tasksList[taskName] = checker;

        CheckQuest(quest);
    }

    void CheckQuest(Quest quest)
    {
        var tasksCompleted = quest.tasksList.Where(x=> x.Value == true).Count();

        if(quest.tasks.Count() == tasksCompleted)
        {
            quest.QuestStatus = QuestState.State.Completed;
        }

    }
}

public class QuestState
{
    public enum State
    {
        Locked,
        Unlocked,
        Completed,
    }
}
