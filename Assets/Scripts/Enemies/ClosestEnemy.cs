using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemy
{
    public Enemy[] enemies;

    public void GetEnemies()
    {
        enemies = Object.FindObjectsOfType<Enemy>();
        Debug.Log(enemies.Length);
    }

    public Enemy GetClosestEnemy(PlayerModel player)
    {
        Enemy closestEnemy = enemies[0];
        foreach (var item in enemies)
        {
            if(item != null && closestEnemy != null)
            {
                float distance = Vector3.Distance(player.transform.position, item.transform.position);
                if (distance < Vector3.Distance(player.transform.position, closestEnemy.transform.position))
                    closestEnemy = item;
            }
        }

        if(closestEnemy!= null)
        {
            Debug.Log(closestEnemy.transform.position);
            return closestEnemy;
        }

        return null;
    }
}
