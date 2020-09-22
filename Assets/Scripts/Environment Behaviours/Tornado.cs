using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    GameObject _target;
    // Start is called before the first frame update
    void Start()
    {
        _target = FindObjectOfType<PlayerModel>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
