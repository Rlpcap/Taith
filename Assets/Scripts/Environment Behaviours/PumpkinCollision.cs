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
            if (!pl.Grounded)
                pl.PumpkinJump();

            SoundManager.PlaySound(SoundManager.Sound.PumpkinHit, transform.position);
        }
    }

    private void OnTriggerEnter(Collider coll)
    {

        if (coll.gameObject.layer == 8)
        {
            _anim.SetTrigger("boing");
            SoundManager.PlaySound(SoundManager.Sound.PumpkinHit, transform.position);
        }
    }
}
