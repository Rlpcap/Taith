using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillGoal : QuestGoal
{
    public int enemyID;

    public KillGoal(QuestGiver quest, int enemyId, string description, bool completed, int currentAmmount, int requiredAmmount)
    {
        this.quest = quest;
        this.enemyID = enemyId;
        this.description = description;
        this.completed = completed;
        this.currentAmmount = currentAmmount;
        this.requiredAmmount = requiredAmmount;
    }

    public override void Init()
    {
        base.Init();
        //add method to event listener onenemydeath
    }

    void EnemyDied(Enemy enemy)
    {
        if (enemy.ID == this.enemyID)
        {
            this.currentAmmount++;
            Evaluate();
        }
    }
}
