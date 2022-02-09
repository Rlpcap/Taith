using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QuestGiver : MonoBehaviour
{
    public List<QuestGoal> goals = new List<QuestGoal>();
    public string questName;
    public string questDescription;
    public bool completed;

    public void CheckGoals()
    {
        if (goals.All(g => g.completed))
        {
            if (completed) GiveReward();
        }
    }

    public void GiveReward()
    {
        //givereward;
    }
}
