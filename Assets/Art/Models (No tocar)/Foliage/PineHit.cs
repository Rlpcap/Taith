using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineHit : MonoBehaviour
{
    public ParticleSystem leafs;
    Animator hit;

    // En el editor le decimos que sonido queremos que reproduzca
    public SoundManager.Sound ChooseSound;

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

            //En este switch, asignamos el sonido a reproducir segun el caso elegido.

            switch (ChooseSound)
            {
                case SoundManager.Sound.PineHit:
                    SoundManager.PlaySound(SoundManager.Sound.PineHit, transform.position);
                    break;
                case SoundManager.Sound.GrassHit:
                    SoundManager.PlaySound(SoundManager.Sound.GrassHit, transform.position);
                    break;
                default:
                    break;
            }
        }
    }
}
