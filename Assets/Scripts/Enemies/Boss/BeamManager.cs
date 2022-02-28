using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamManager : MonoBehaviour, IUpdate
{
    LineRenderer _myself;
    Transform _boss;
    Transform _subject;

    private void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        _myself = GetComponent<LineRenderer>();
    }

    public void OnUpdate()
    {
        SetPosition();
    }

    void SetPosition()
    {
        if(_boss && _subject)
        {
            _myself.SetPosition(0, _boss.transform.position + new Vector3(0, 5, 0));
            _myself.SetPosition(1, transform.position + new Vector3(0, 3, 0));
        }
    }

    public BeamManager SetBoss(Transform b)
    {
        _boss = b;
        return this;
    }

    public BeamManager SetSubject(Transform s)
    {
        _subject = s;
        return this;
    }
}
