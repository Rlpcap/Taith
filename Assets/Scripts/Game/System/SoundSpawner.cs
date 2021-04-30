using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSpawner : MonoBehaviour
{
    public static SoundSpawner instance
    {
        get
        {
            return _instance;
        }
    }

    static SoundSpawner _instance;


    public SoundObject prefab;
    public int initialStock;
    public ObjectPool<SoundObject> pool;

    private void Start()
    {
        _instance = this;
        pool = new ObjectPool<SoundObject>(SoundFactory, SoundObject.SoundObjectOn, SoundObject.SoundObjectOff, initialStock, true);
    }

    public SoundObject SoundFactory()
    {
        Debug.Log("object Instantiated");
        var obj = Instantiate(prefab);
        return obj;
    }

    public void ReturnSound(SoundObject obj)
    {
        pool.ReturnObject(obj);
    }

}
