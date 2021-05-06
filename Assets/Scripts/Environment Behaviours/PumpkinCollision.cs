using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpkinCollision : MonoBehaviour
{
    Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnCollisionEnter(Collision coll)
    {
        var pl = coll.gameObject.GetComponent<PlayerModel>();
        if (pl)
        {
            _anim.SetTrigger("boing");
            if(!pl.Grounded)
                pl.PumpkinJump();
        }
    }
}
