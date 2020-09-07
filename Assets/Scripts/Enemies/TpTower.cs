using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpTower : MonoBehaviour, IUpdate
{
    public TpTower target;
    public PlayerModel playerModel;
    public LayerMask playerMask;
    public GameObject spawnPos;

    private bool _towerRange;

    private void Start()
    {
        playerModel = FindObjectOfType<PlayerModel>();
        UpdateManager.Instance.AddElementUpdate(this);
    }

    public void OnUpdate()
    {
        CheckTower();
    }


    void CheckTower()
    {
        if (playerModel.CanTp)
        {
            UseTower();
        }
    }

    private void UseTower()
    {
        _towerRange = Physics.CheckSphere(this.transform.position, 5f, playerMask);
        if (_towerRange)
        {
            playerModel.TP(target.spawnPos.transform.position);
            //MoveObject(playerModel);
            //playerModel.canTp = false;
        }
    }

    //private void MoveObject(PlayerModel playerModel)
    //{
    //    if (playerModel != null)
    //        playerModel.transform.position = target.spawnPos.transform.position;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 5f);
    }
}
