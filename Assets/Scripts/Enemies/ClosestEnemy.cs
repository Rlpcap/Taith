using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemy
{
    float radius = 10f;

    public Enemy GetClosestEnemy(PlayerModel player)
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, radius);
        Enemy closestEnemy = null;
        foreach (var hitCollider in hitColliders)
        {
            if(hitCollider.gameObject.GetComponent<Enemy>())
            {
                Enemy enemy = hitCollider.gameObject.GetComponent<Enemy>();
                if (hitCollider != null)
                {
                    float distance = Vector3.Distance(player.transform.position, hitCollider.transform.position);

                    if (closestEnemy != null)
                    {
                        if (distance < Vector3.Distance(player.transform.position, closestEnemy.transform.position))
                            closestEnemy = enemy;
                    }
                    else
                    {
                        closestEnemy = enemy;
                    }
                }
            }
        }

        if(closestEnemy!= null)
        {
            //Debug.Log(closestEnemy.transform.position);
            return closestEnemy;
        }

        return null;
    }
}
