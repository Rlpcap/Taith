using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView : MonoBehaviour
{
    //Poderes
    public GameObject iceLaserBeam, stopTimePrefab, earthShield;

    public float powerFadeSpeed;
    public Image powerImage;
    //public List<GameObject> vines = new List<GameObject>();
    public List<GameObject> powersUI = new List<GameObject>();
    public GameObject crystal, littleStopTimeBubble;
    Renderer _crystalRenderer;
    Color _crystalStartColor;
    public List<Color> CrystalColors = new List<Color>();
    public List<GameObject> crystals = new List<GameObject>();
    public Text powerText;
    public ParticleSystem dust, fireTrail, onFire, onFreeze;

    Animator _anim;
    PlayerModel _playermodel;

    GameObject _currentImage;
    GameObject _currentCrystal;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _playermodel = GetComponent<PlayerModel>();
        _crystalRenderer = crystal.GetComponent<Renderer>();
        _crystalStartColor = _crystalRenderer.material.color;
    }

    public void GroundCheck(bool grounded)
    {
        _anim.SetBool("grounded", grounded);

        if ((_anim.GetCurrentAnimatorStateInfo(0).IsName("Fall") || _anim.GetCurrentAnimatorStateInfo(0).IsName("DobleJump Idle")) && grounded)
        {
            _anim.SetTrigger("land");
            dust.Play();
            StartCoroutine(ResetAllTriggers());
        }
    }

    public void CallOnStoppedInTime(float time)
    {
        StartCoroutine(OnStoppedInTime(time));
    }

    IEnumerator OnStoppedInTime(float duration)
    {
        littleStopTimeBubble.SetActive(true);
        yield return new WaitForSeconds(duration);
        littleStopTimeBubble.SetActive(false);
    }

    public void CallOnFreeze(float time)
    {
        onFreeze.Play();
    }
    public void CallOnFire(float time)
    {
        StartCoroutine(OnFire(time));
    }

    IEnumerator OnFire(float duration)
    {
        onFire.Play();
        yield return new WaitForSeconds(duration);
        onFire.Stop();
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

    public void SpawnEarthShield(float duration)
    {
        StartCoroutine(EarthShield(duration));
    }

    IEnumerator EarthShield(float time)
    {
        //spawnear escudo alrededor del player
        earthShield.SetActive(true);
        yield return new WaitForSeconds(time);
        earthShield.SetActive(false);
        //despawnear escudo alrededor del player
    }

    public void PlayFireDash(float duration)
    {
        StartCoroutine(FireDash(duration));
    }

    IEnumerator FireDash(float time)
    {
        fireTrail.Play();
        yield return new WaitForSeconds(time);
        fireTrail.Stop();
    }

    public void NewPower(int index)
    {
        //StopCoroutine(ShowPower());
        if (_currentImage)
            _currentImage.SetActive(false);
        if (_currentCrystal)
            _currentCrystal.gameObject.SetActive(false);

        crystals[index].gameObject.SetActive(true);
        _currentCrystal = crystals[index];
        _crystalRenderer.material.color = CrystalColors[index];
        _crystalRenderer.material.SetColor("_EmissionColor", CrystalColors[index] * 5);
        StartCoroutine(ShowPower(index));
    }

    IEnumerator ShowPower(int index)
    {
        float myAlpha = 0.5f;
        _currentImage = powersUI[index];

        _currentImage.SetActive(true);
        //foreach (var go in vines)
        //{
        //    go.SetActive(true);
        //}
        while(myAlpha < 1)
        {
            myAlpha += powerFadeSpeed;
            powerImage.color = new Color(powerImage.color.r, powerImage.color.g, powerImage.color.b, myAlpha);
            yield return null;
        }

        //yield return new WaitForSeconds(2);

        //while (myAlpha > 0)
        //{
        //    myAlpha -= powerFadeSpeed;
        //    powerImage.color = new Color(powerImage.color.r, powerImage.color.g, powerImage.color.b, myAlpha);
        //    yield return null;
        //}
        //currentImage.SetActive(false);
        //foreach (var go in vines)
        //{
        //    go.SetActive(false);
        //}
    }

    public void UsePower()
    {
        StartCoroutine(HidePower());
        _currentCrystal.gameObject.SetActive(false);
        _crystalRenderer.material.color = _crystalStartColor;
        _crystalRenderer.material.SetColor("_EmissionColor", Color.black);
    }

    IEnumerator HidePower()
    {
        float myAlpha = 1f;

        while (myAlpha > 0.5f)
        {
            myAlpha -= powerFadeSpeed;
            powerImage.color = new Color(powerImage.color.r, powerImage.color.g, powerImage.color.b, myAlpha);
            yield return null;
        }
        _currentImage.SetActive(false);
        //foreach (var go in vines)
        //{
        //    go.SetActive(false);
        //}
    }
}
