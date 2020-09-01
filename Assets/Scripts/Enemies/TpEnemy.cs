using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpEnemy : Enemy
{
    public override void Action()
    {
        this.gameObject.SetActive(false);
        playerModel.canTp = true;
    }
}
