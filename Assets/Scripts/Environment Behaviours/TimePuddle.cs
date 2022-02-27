using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePuddle : MonoBehaviour, IUpdate
{
    float maxDissolveRadius;
    float currentRadius;


    private void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        GetComponent<Renderer>().material.SetFloat("_radius", 0);
    }

    public void OnUpdate()
    {
        UpdateEnemyPos();
    }

    void UpdateEnemyPos()
    {
        GetComponent<Renderer>().material.SetVector("_enemyPos1", transform.position);
    }

    public void TimeOn()
    {
        StartCoroutine(ExpandMat(.1f));
    }

    public void TimeOff()
    {
        if(currentRadius > 0)
            StartCoroutine(DissolveMat(.1f));
    }

    public IEnumerator ExpandMat(float dissolveSpeed)
    {
        float radius = 0;
        while (radius < maxDissolveRadius)
        {
            radius += dissolveSpeed;
            currentRadius = radius;
            GetComponent<Renderer>().material.SetFloat("_radius", radius);
            yield return null;
        }
    }

    public IEnumerator DissolveMat(float dissolveSpeed)
    {
        var radius = currentRadius;
        while (radius > 0)
        {
            radius -= dissolveSpeed;
            GetComponent<Renderer>().material.SetFloat("_radius", radius);
            yield return null;
        }
    }

    public TimePuddle SetMaxRadius(float r)
    {
        maxDissolveRadius = r;
        return this;
    }

    private void OnDestroy()
    {
        UpdateManager.Instance.RemoveElementUpdate(this);
    }
}
