using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerView : MonoBehaviour
{
    public GameObject iceLaserBeam;
    public GameObject stopTimePrefab;

    public void SpawnStopTimeBubble(float time)
    {
        StartCoroutine(StopTimeBubble(time));
    }

    IEnumerator StopTimeBubble(float duration)
    {
        var b = Instantiate(stopTimePrefab, transform.position, transform.rotation);

        yield return new WaitForSeconds(duration);

        Destroy(b.gameObject);
    }

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
