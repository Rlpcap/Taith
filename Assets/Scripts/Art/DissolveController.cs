using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissolveController : MonoBehaviour
{
    public Material mat;
    public Transform pos;
    void Start()
    {
        
    }

    void Update()
    {
        mat.SetVector("_enemyPos", pos.position);
    }
}
