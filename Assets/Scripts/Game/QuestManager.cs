using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuestManager : Singleton<QuestManager>
{
    public List<Quest> _listOfQuests = new List<Quest>();


   /* void Start()
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
    }*/

    void Update()
    {
       /* Debug.Log(_listOfQuests[0].QuestName);
        Debug.Log(""+_listOfQuests[0].tasksList[_listOfQuests[0].tasks[0]]);
        Debug.Log("Estado de la quest: "+ _listOfQuests[0].QuestStatus.ToString());*/

        Debug.Log(DebugQuest("The Hat Quest"));
    }

    public void AddQuestToList(Quest quest)
    {
        _listOfQuests.Add(quest);
        for (int i = 0; i < quest.tasks.Length; i++)
        {
            quest.tasksList.Add(quest.tasks[i],false);
        }

        CheckQuestStatus(quest.QuestName,QuestState.State.Locked);
    }

    public bool CheckQuestStatus(string questName,QuestState.State statusCheck)
    {
        var quest = _listOfQuests.Where(x=> x.QuestName== questName).First();

        if(quest.QuestStatus == statusCheck)
            return true;
        else
            return false;
    }


    public void ChangeQuestStatus(string questName, QuestState.State newStatus)
    {
        var quest = _listOfQuests.Where(x=> x.QuestName == questName).First();
        
        quest.QuestStatus = newStatus;

        CheckQuest(quest);

        //checkeo si la quest esta completa en un hipotetico caso en el que el jugador haya hecho las tareas antes de empezar la quest

    }
    public void CheckTask(string questName, string taskName, bool checker)
    {
        var quest = _listOfQuests.Where(x=>x.QuestName == questName).First();

        quest.tasksList[taskName] = checker;

        CheckQuest(quest);
    }

    public void CheckQuest(Quest quest)
    {
        var tasksCompleted = quest.tasksList.Where(x=> x.Value == true).Count();

        if(quest.tasks.Count() == tasksCompleted)
        {
            quest.QuestStatus = QuestState.State.Completed;

        }
    }

    public bool GiveReward(QuestReward reward)
    {
        if(reward.rewardGiven)
            return false;

        switch (reward.rewardType)
        {
            default:
            {
                return false;
            }

            case QuestReward.RewardType.Stat:
            {
                reward.rewardGiven=true;
                return true;
            }
        }
        
    }

    string DebugQuest(string quest)
    {
        var questDebugged = _listOfQuests.Where(x=>x.QuestName == quest).First();

        var debug = "" + questDebugged.QuestName+ " " + " " + questDebugged.QuestStatus.ToString();

        return debug;
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
