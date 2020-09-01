using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpTower : MonoBehaviour
{
    public TpTower target;
    public PlayerModel playerModel;
    public LayerMask playerMask;

    private bool _towerRange;

    private void Awake()
    {
        playerModel = FindObjectOfType<PlayerModel>();
    }

    void Update()
    {
        CheckTower();
    }

    void CheckTower()
    {
        if (playerModel.canTp)
        {
            UseTower();
        }
    }

    private void UseTower()
    {
        _towerRange = Physics.CheckSphere(this.transform.position, 5f, playerMask);
        if (_towerRange)
        {
            MoveObject(playerModel);
            playerModel.canTp = false;
        }

    }

    private void MoveObject(PlayerModel playerModel)
    {
        if (playerModel != null)
            playerModel.transform.position = target.transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 5f);
    }
}
