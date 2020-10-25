using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    //Poderes
    public GameObject iceLaserBeam, stopTimePrefab;

    public Image powerImage;
    public Text powerText;

    Animator _anim;


    void Start()
    {
        //_anim = GetComponent<Animator>();
    }

    public void RunAnim(float vel)
    {
        _anim.SetFloat("vel", vel);
    }

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

    public void NewPower()
    {
        StartCoroutine(ShowPower());
    }

    IEnumerator ShowPower()
    {
        float myAlpha = 0f;

        while(myAlpha < 1)
        {
            myAlpha += .1f;
            powerImage.color = new Color(powerImage.color.r, powerImage.color.g, powerImage.color.b, myAlpha);
            powerText.color = new Color(powerText.color.r, powerText.color.g, powerText.color.b, myAlpha);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        while (myAlpha > 0)
        {
            myAlpha -= .1f;
            powerImage.color = new Color(powerImage.color.r, powerImage.color.g, powerImage.color.b, myAlpha);
            powerText.color = new Color(powerText.color.r, powerText.color.g, powerText.color.b, myAlpha);
            yield return null;
        }
    }
}
