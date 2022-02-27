using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class QuestManager : Singleton<QuestManager>
{
    public List<Quest> _listOfQuests = new List<Quest>();

    public List<QuestGiver> quests = new List<QuestGiver>();

    public List<QuestInfo> questsInfo = new List<QuestInfo>();
    

  /*  public List<QuestGiver> Quests
    {
        get { return quests; }
        set
        {
            quests = value;
            UIEventHandler.UpdateQuestsUI();
            Debug.Log("Call Event!!!");
        }
    }*/
    PlayerView _pv;

    public QuestManager Set(PlayerView pv)
    {
        _pv = pv;
        return this;
    }

    public void SaveQuests()
    {
        if (questsInfo.Count != quests.Count)
        {
            questsInfo = new List<QuestInfo>(quests.Count - questsInfo.Count);
        }
        foreach (var q in quests)
        {
            foreach (var i in questsInfo)
            {

                if (i.questName != "")
                {
                    i.questName = q.questName;
                    i.questDescription = q.questDescription;
                    i.completed = q.completed;
                    i.itemReward = q.itemReward;
                    i.pv = q.pv;
                }
                else if (q.questName == i.questName)
                {
                    i.questName = q.questName;
                    i.questDescription = q.questDescription;
                    i.completed = q.completed;
                    i.itemReward = q.itemReward;
                    i.pv = q.pv;
                }
            }
        }
    }

    public void LoadQuests()
    {
        if (quests.Count != 0)
        {
            foreach (var q in quests)
            {
                foreach (var i in questsInfo)
                {
                    if (q.questName == i.questName)
                    {
                        i.questName = q.questName;
                        i.questDescription = q.questDescription;
                        i.completed = q.completed;
                        i.itemReward = q.itemReward;
                        i.pv = q.pv;
                    }
                }
            }

        }
    }

    #region OLD QUEST SYSTEM
    public void AddQuestToList(Quest quest)
    {
        _listOfQuests.Add(quest);
        for (int i = 0; i < quest.tasks.Length; i++)
        {
            quest.tasksList.Add(quest.tasks[i], false);
        }

        CheckQuestStatus(quest.QuestName, QuestState.State.Locked);
    }

    public bool CheckQuestStatus(string questName, QuestState.State statusCheck)
    {
        var quest = _listOfQuests.Where(x => x.QuestName == questName).FirstOrDefault();

        if (quest != default)
        {
            if (quest.QuestStatus == statusCheck)
                return true;
            else
                return false;
        }
        else
            return false;
    }


    public void ChangeQuestStatus(string questName, QuestState.State newStatus)
    {
        var quest = _listOfQuests.Where(x => x.QuestName == questName).FirstOrDefault();

        if (quest != default)
        {
            quest.QuestStatus = newStatus;

            if (newStatus == QuestState.State.Unlocked)
                //_pv.ShowQuestsUI();

                CheckQuest(quest);
        }


        //checkeo si la quest esta completa en un hipotetico caso en el que el jugador haya hecho las tareas antes de empezar la quest

    }
    public void CheckTask(string questName, string taskName, bool checker)
    {
        var quest = _listOfQuests.Where(x => x.QuestName == questName).FirstOrDefault();

        if (quest != default)
        {
            quest.tasksList[taskName] = checker;

            CheckQuest(quest);
        }
    }

    public bool CurrentTask(string questName, string taskName, bool checker)
    {
        var quest = _listOfQuests.Where(x => x.QuestName == questName).FirstOrDefault();

        if (quest != default && quest.tasksList[taskName] == checker)
            return true;
        else
            return false;
    }

    public void CheckQuest(Quest quest)
    {
        var tasksCompleted = quest.tasksList.Where(x => x.Value == true).Count();

        if (quest.tasks.Count() == tasksCompleted)
        {
            quest.QuestStatus = QuestState.State.Completed;
            //_pv.ShowQuestsUI();

        }
    }

    public bool GiveReward(QuestReward reward)
    {
        if (reward.rewardGiven)
            return false;

        switch (reward.rewardType)
        {
            default:
                {
                    return false;
                }

            case QuestReward.RewardType.Stat:
                {
                    reward.rewardGiven = true;
                    return true;
                }
        }

    }

    string DebugQuest(string quest)
    {
        var questDebugged = _listOfQuests.Where(x => x.QuestName == quest).First();

        var debug = "" + questDebugged.QuestName + " " + " " + questDebugged.QuestStatus.ToString();

        return debug;
    }
    #endregion
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

public class QuestInfo : QuestGiver
{
    /*  public List<QuestGoal> goals { get; set; }
      public string questName;
      public string questDescription;
      public bool completed;

      public Item itemReward;

      public PlayerView pv;*/
}
