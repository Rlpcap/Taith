using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{

    public static void PlaySound(AudioClip clip)
    {
        GameObject soundObj = new GameObject("Sound");
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();
        if(clip!=null)
            audioSource.PlayOneShot(clip);

        if (!audioSource.isPlaying)
            GameObject.Destroy(soundObj);
    }
}
