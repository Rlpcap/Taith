﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageController : MonoBehaviour
{
    public ParticleSystem leafs;
    public bool hasSound;
    public SoundManager.Sound chooseSound;

    Animator hit;


    void Start()
    {
        hit = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "MeleeCollider")
        {
            if (GetComponentInChildren<ParticleSystem>() != null)
                leafs.Play();
            hit.SetTrigger("hit");

            if (hasSound)
                SoundManager.PlaySound(chooseSound, transform.position);

            if (gameObject.tag == "Dummy" && FindObjectOfType<HitDummyQuest>())
                InventoryController.Instance.GiveItem("HitDummy");
        }
    }
}
