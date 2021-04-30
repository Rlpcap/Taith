using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour,IUpdate
{
    public AudioSource audioSource;
    float _timer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
    }

    public void OnUpdate()
    {
        if(!audioSource.isPlaying)
            SoundSpawner.instance.ReturnSound(this);
    }

    public static void SoundObjectOn(SoundObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    public static void SoundObjectOff(SoundObject obj)
    {
        obj.gameObject.SetActive(false);
    }

}
