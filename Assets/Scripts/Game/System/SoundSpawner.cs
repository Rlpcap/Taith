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
    public ObjectPool<SoundObject> pool;
    public int initialStock = 5;


    private void Awake()
    {
        prefab = Resources.Load<SoundObject>("prefabs/Audio");

    }

    private void Start()
    {
        _instance = this;
        pool = new ObjectPool<SoundObject>(SoundFactory, SoundObject.SoundObjectOn, SoundObject.SoundObjectOff, initialStock, true);
    }

    public SoundObject SoundFactory()
    {
        return Instantiate(prefab);
    }

    public void ReturnSound(SoundObject obj)
    {
        pool.ReturnObject(obj);
    }

}
