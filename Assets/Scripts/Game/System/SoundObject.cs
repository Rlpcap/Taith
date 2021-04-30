using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    public AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

   /* private void Start()
    {
        UpdateManager.Instance.AddElementUpdate(this);
    }
    */
    void Update()
    {

        if (!audioSource.isPlaying)
        {
            SoundSpawner.instance.pool.ReturnObject(this);
           // UpdateManager.Instance.RemoveElementUpdate(this);
        }

    }

   /* public void OnUpdate()
    {
    }*/

    public static void SoundObjectOn(SoundObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    public static void SoundObjectOff(SoundObject obj)
    {
        obj.gameObject.SetActive(false);
    }

}
