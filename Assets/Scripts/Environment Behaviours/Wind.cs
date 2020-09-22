using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wind : MonoBehaviour, IUpdate, IFixedUpdate
{
    public PlayerModel playermodel;


    // en que direccion queremos que sople el viento
    public Vector3 dir;


    // Cuanto fuerza queremos que haga le viento(lo ajustamos en el editor)
    public float force;



    public float timeToStopAndRestart;

    public bool windAlwaysOn;

    private bool _startWind;

    public Text wind;

    private Rigidbody _playerRb;

    private void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
        UpdateManager.Instance.AddElementFixedUpdate(this);
        playermodel = FindObjectOfType<PlayerModel>();
        _playerRb = playermodel.GetComponent<Rigidbody>();
        if(!windAlwaysOn)
            StartCoroutine(StopAndRestartWind(timeToStopAndRestart));
    }

    public void OnUpdate()
    {
        //feedback vago para mostrarnos cuando esta soplando el viento
        wind.text = "Wind Status: " + _startWind.ToString();

        //Preguntamos si queremos que el viento siempre este soplando, si la variable es true la fuerza del viento siempre se aplica
        if (windAlwaysOn)
            _startWind = true;
    }


    // el viento sopla por x cantidad de tiempo y deja de soplar por la misma cantidad de tiempo(dicha cantidad de tiempo la seteamos en el editor ajustando la variable timetostopandrestart
    IEnumerator StopAndRestartWind(float t)
    {
        if(!windAlwaysOn)
        {
            Debug.Log("Start");
            _startWind = true;
            yield return new WaitForSeconds(t);
            _startWind = false;
            yield return new WaitForSeconds(t);
            StartCoroutine(StopAndRestartWind(t));
        }
    }

    //aplicamos la fuerza del viento en el fixedupdate
    public void OnFixedUpdate()
    {
        ApplyWind(dir, _startWind);
    }


    private void ApplyWind(Vector3 windDir, bool canWind)
    {
        //si el bool _startwind es verdadero, aplicamos la fuerza al RB del playermodel
        if (canWind)
            _playerRb.AddForce(windDir * force, ForceMode.Acceleration);
    }
}
