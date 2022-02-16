using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGoal
{
    public QuestGiver quest;
    public string description;
    public bool completed;
    public int currentAmmount;
    public int requiredAmmount;


    public virtual void Init()
    {
        //defualt init;
    }
    public void Evaluate()
    {
        if (currentAmmount >= requiredAmmount)
        {
            Complete();
            Debug.Log("CurrentAmmount is equal or more than requiredAmmount. Checking goals...");
        }
    }

    public void Complete()
    {
        completed = true;
        this.quest.CheckGoals();
    }
}


