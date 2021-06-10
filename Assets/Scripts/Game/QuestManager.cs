using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuestManager : Singleton<QuestManager>
{
    List<Quest> listOfQuests = new List<Quest>();


    void Start()
    {
        listOfQuests.Add(new Quest());
        listOfQuests[0].QuestName = "Nombre de la quest";
        listOfQuests[0].tasks.Add("Primera Tarea", false);

        foreach (var q in listOfQuests)
        {
            q.QuestStatus = QuestState.State.Locked;
        }
    }

    void Update()
    {
        Debug.Log(listOfQuests[0].QuestName);
        Debug.Log(""+listOfQuests[0].tasks["Primera Tarea"]);
        Debug.Log("Estado de la quest: "+ listOfQuests[0].QuestStatus.ToString());

        
    }

    public void ChangeQuestStatus(string questName, QuestState.State newStatus)
    {
        var quest = listOfQuests.Where(x=> x.QuestName == questName).First();
        
        quest.QuestStatus = newStatus;

        CheckQuest(quest);

    }
    public void CheckTask(string questName, string taskName, bool checker)
    {
        var quest = listOfQuests.Where(x=>x.QuestName == questName).First();

        quest.tasks[taskName] = checker;

        CheckQuest(quest);
    }

    void CheckQuest(Quest quest)
    {
        var tasksCompleted = quest.tasks.Where(x=> x.Value == true).Count();

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
