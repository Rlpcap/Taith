using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound
    {
        PlayerJump,
        PlayerAttack,
        PlatformShake,
        EnemyDeath,
    
    }

    public static void PlaySound(Sound sound)
    {
        Debug.Log("Creating sound!");
        GameObject soundObj = new GameObject("Sound");
        AudioSource audioSource = soundObj.AddComponent<AudioSource>();

        audioSource.PlayOneShot(GetClip(sound));
    }


    public static AudioClip GetClip(Sound sound)
    {
        foreach (GameManager.SoundAudioClip soundAudioClip in GameManager.Instance.soundClips)
        {
            if(soundAudioClip.sound == sound)
            {
                return soundAudioClip.audioClip;
            }

        }
            return null;
    }
}
