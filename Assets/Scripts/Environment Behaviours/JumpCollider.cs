using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCollider : MonoBehaviour
{
    private void OnCollisionEnter(Collision coll)
    {
        var pl = coll.gameObject.GetComponent<PlayerModel>();
        if (pl)
        {
            GetComponentInParent<Animator>().SetTrigger("hit");
            if (!pl.Grounded)
            {
                pl.ObjectJump();
            }
        }
    }
}
