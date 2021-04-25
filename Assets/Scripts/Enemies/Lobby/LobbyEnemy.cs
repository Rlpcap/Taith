using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyEnemy : MonoBehaviour
{
    protected PlayerModel _playerModel;
    public UIIndex myPower;
    Animator _anim;

    void Start()
    {
        _playerModel = FindObjectOfType<PlayerModel>();
        _anim = GetComponentInChildren<Animator>();
    }

    protected virtual void GetPower()
    {
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "MeleeCollider")
        {
            coll.gameObject.SetActive(false);
            _anim.SetTrigger("hit");
            GetPower();
        }
    }
}
