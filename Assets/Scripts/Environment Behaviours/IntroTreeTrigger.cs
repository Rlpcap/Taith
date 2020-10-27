using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroTreeTrigger : MonoBehaviour, IUpdate
{
    public FallingBridgeRB tree;
    public float fallDist;
    bool _checkingDist = true;
    PlayerModel pl;

    private void Start()
    {
        pl = FindObjectOfType<PlayerModel>();
        UpdateManager.Instance.AddElementUpdate(this);
    }

    public void OnUpdate()
    {
        if (_checkingDist)
            CheckDist();
    }

    void CheckDist()
    {
        if (Vector3.Distance(pl.transform.position, transform.position) < fallDist)
        {
            _checkingDist = false;
            tree.Push();
        }
    }

    private void OnTriggerEnter(Collider coll)
    {
        var deadTree = coll.GetComponent<FallingBridgeRB>();

        if (deadTree)
        {
            deadTree.Freeze();
            UpdateManager.Instance.RemoveElementUpdate(this);
            Destroy(gameObject);
        }
    }
}
