using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public Transform attackPoint;
    public float xRandomDist;
    public float zRandomDist;
    public int attackAmount;
    public float attackPrepareT;
    public Boss boss;
    public bool switchTrigger;

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();

        if (pl)
        {
            if (switchTrigger)
            {
                boss.Switch();
                Destroy(gameObject);
            }
            else
            {
                boss.Attack(attackPoint, attackAmount, attackPrepareT, xRandomDist, zRandomDist);
                Destroy(gameObject);
            }
        }
    }
}
