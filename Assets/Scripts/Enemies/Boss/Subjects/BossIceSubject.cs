using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIceSubject : BossSubjects
{
    protected override void Start()
    {
        base.Start();
        _myPowerAction = _pl.IceSpell;
    }
}
