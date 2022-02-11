using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWindEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Die(1.25f));
    }

    IEnumerator Die(float t)
    {
        yield return UpdateManager.WaitForSecondsCustom(t);
        Destroy(gameObject);
    }
}
