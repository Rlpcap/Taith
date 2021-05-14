using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageController : MonoBehaviour
{
    public ParticleSystem leafs;
    Animator hit;

    public SoundManager.Sound chooseSound;

    void Start()
    {
        hit = GetComponent<Animator>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            if(GetComponentInChildren<ParticleSystem>() != null)
                leafs.Play();
            hit.SetTrigger("hit");

            switch (chooseSound)
            {
                case SoundManager.Sound.TreeMove:
                    SoundManager.PlaySound(SoundManager.Sound.TreeMove, transform.position);
                    break;
                case SoundManager.Sound.DummyHit:
                    SoundManager.PlaySound(SoundManager.Sound.DummyHit, transform.position);
                    break;
                default:
                    break;
            }
        }
    }
}
