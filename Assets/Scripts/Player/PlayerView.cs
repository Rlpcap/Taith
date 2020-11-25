using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    //Poderes
    public GameObject iceLaserBeam, stopTimePrefab;

    public float powerFadeSpeed;
    public Image powerImage;
    public List<GameObject> powersUI = new List<GameObject>();
    public Text powerText;

    Animator _anim;
    PlayerModel _playermodel;


    void Start()
    {
        _anim = GetComponent<Animator>();
        _playermodel = GetComponent<PlayerModel>();
    }

    public void GroundCheck(bool grounded)
    {
        _anim.SetBool("grounded", grounded);

        if (_anim.GetCurrentAnimatorStateInfo(0).IsName("JumpIdle") && grounded)
        {
            _anim.SetTrigger("land");
            StartCoroutine(ResetAllTriggers());
        }
    }

    public void Attack()
    {
        _anim.SetTrigger("attack");
        StartCoroutine(ResetAllTriggers());
    }

    public void Cast()
    {
        _anim.SetTrigger("cast");
        StartCoroutine(ResetAllTriggers());
    }

    public void Jump(bool grounded)
    {
        if (grounded)
            _anim.SetTrigger("jump");
        else
            _anim.SetTrigger("airjump");
        StartCoroutine(ResetAllTriggers());
    }

    public void RunAnim(float vel)
    {
        _anim.SetFloat("vel", vel);
    }

    IEnumerator ResetAllTriggers()
    {
        yield return null;

        var triggers = _anim.parameters.Where(p => p.type == AnimatorControllerParameterType.Trigger);

        foreach (var p in triggers)
            _anim.ResetTrigger(p.name);
    }

    #region trigger resets
    public void ResetLandTrigger()
    {
        _anim.ResetTrigger("land");
    }

    public void ResetAttackTrigger()
    {
        _anim.ResetTrigger("attack");
    }

    public void ResetCastTrigger()
    {
        _anim.ResetTrigger("cast");
    }

    public void ResetJumpTrigger()
    {
        _anim.ResetTrigger("jump");
    }
    #endregion

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
        var b = Instantiate(iceLaserBeam.gameObject);
        b.transform.position = _playermodel.laserRayPos.transform.position;
        b.transform.forward = _playermodel.laserRayPos.transform.forward;
        yield return new WaitForSeconds(duration);
    }

    public void NewPower(int index)
    {
        //StopCoroutine(ShowPower());
        StartCoroutine(ShowPower(index));
    }

    IEnumerator ShowPower(int index)
    {
        float myAlpha = 0f;
        var currentImage = powersUI[index];

        currentImage.SetActive(true);
        while(myAlpha < 1)
        {
            myAlpha += powerFadeSpeed;
            powerImage.color = new Color(powerImage.color.r, powerImage.color.g, powerImage.color.b, myAlpha);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        while (myAlpha > 0)
        {
            myAlpha -= powerFadeSpeed;
            powerImage.color = new Color(powerImage.color.r, powerImage.color.g, powerImage.color.b, myAlpha);
            yield return null;
        }
        currentImage.SetActive(false);
    }
}
