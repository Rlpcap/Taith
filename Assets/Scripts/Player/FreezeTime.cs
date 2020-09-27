using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTime : MonoBehaviour ,IPower
{
    PlayerModel _player;
    float _timeStopRange;
    LayerMask _groundLayer;
    float _freezeTime;

    public FreezeTime(PlayerModel player,float timeStopRange, LayerMask groundLayer, float freezeTime)
    {
        _player = player;
        _timeStopRange = timeStopRange;
        _groundLayer = groundLayer;
        _freezeTime = freezeTime;
    }

    public void UsePower()
    {
        var propsInArea = Physics.OverlapSphere(_player.transform.position, _timeStopRange, _groundLayer);
        foreach (var prop in propsInArea)
        {
            if (prop.GetComponent<IFreezable>() != null)
                StartCoroutine(prop.GetComponent<IFreezable>().FreezeTime(_freezeTime));
        }
    }
}
