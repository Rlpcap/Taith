﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageController : MonoBehaviour
{
    public ParticleSystem leafs;
    Animator hit;

    void Start()
    {
        hit = GetComponent<Animator>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            leafs.Play();
            hit.SetTrigger("hit");
        }
    }
}