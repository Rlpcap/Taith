using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ClosestEnemy
{
    float radius = 5f;


    public Enemy GetClosestEnemy(PlayerModel player)
    {
        Collider[] hitColliders = Physics.OverlapSphere(player.transform.position, radius);

        Enemy closestEnemy = null;

        var getClosestEnemy = hitColliders.Where(x => x.GetComponent<Enemy>()).OrderBy(x => Vector3.Distance(player.transform.position, x.transform.position)).FirstOrDefault();

        if (getClosestEnemy)
        {
            closestEnemy = getClosestEnemy.GetComponent<Enemy>();
            return closestEnemy;
        }

        return null;
    }
}
