using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Totem : MonoBehaviour
{
    public List<TotemPlatform> linkedPlatforms = new List<TotemPlatform>();
    Animator _anim;
    public Material _mat;

    public void Start()
    {
        _anim = GetComponentInChildren<Animator>();
        _mat.SetColor("_EmissionColor", Color.white * 5);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.GetComponent<PlayerModel>())
        {
            _mat.SetColor("_EmissionColor", Color.green * 5);
            _anim.SetTrigger("pressed");
            foreach (var item in linkedPlatforms)
            {
                item.Inactive = false;
            }
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.GetComponent<PlayerModel>())
        {
            _mat.SetColor("_EmissionColor", Color.white * 5);
            _anim.SetTrigger("exit");
            foreach (var item in linkedPlatforms)
            {
                item.Inactive = true;
            }
        }
    }
}
