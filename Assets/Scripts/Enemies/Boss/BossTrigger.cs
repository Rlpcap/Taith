using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrigger : MonoBehaviour
{
    public Transform attackPoint;
    public Boss boss;
    public bool switchTrigger;

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();

        if (pl)
        {
            if (switchTrigger)
                boss.Switch();
            else
                boss.Attack(attackPoint.position);
        }
    }
}
