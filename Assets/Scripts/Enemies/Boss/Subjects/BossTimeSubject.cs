using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTimeSubject : BossSubjects
{
    protected override void Start()
    {
        base.Start();
        _myPowerAction = _pl.StopTime;
    }
}
