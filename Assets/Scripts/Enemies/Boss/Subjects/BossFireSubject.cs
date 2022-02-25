using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireSubject : BossSubjects
{
    protected override void Start()
    {
        base.Start();
        _myPowerAction = _pl.Dash;
    }
}
