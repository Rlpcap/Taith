using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public GameObject iceLaserBeam;

    public void SpawnLaser(float duration)
    {
        StartCoroutine(IceLaser(duration));
    }

    IEnumerator IceLaser(float duration)
    {
        iceLaserBeam.SetActive(true);
        yield return new WaitForSeconds(duration);
        iceLaserBeam.SetActive(false);
    }
}
