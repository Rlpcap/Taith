using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineHit : MonoBehaviour
{
    public ParticleSystem leafs;
    public bool hasSound;
    public SoundManager.Sound ChooseSound;

    Animator hit;


    void Start()
    {
        hit = GetComponent<Animator>();
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == 8)
        {
            if (GetComponentInChildren<ParticleSystem>() != null)
                leafs.Play();
            hit.SetTrigger("hit");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if (GetComponentInChildren<ParticleSystem>() != null)
                leafs.Play();
            hit.SetTrigger("hit");

            if(hasSound)
                SoundManager.PlaySound(ChooseSound, transform.position);
        }
    }
}
