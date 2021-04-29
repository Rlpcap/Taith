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

    public static Dictionary<Sound, float> soundTimer;

    public static GameObject oneShotGameObject;
    public static AudioSource oneShotAudioSource;

    static bool CanPlaySound(Sound sound)
    {
        if (soundTimer != null)
        {
            switch (sound)
            {
                default: return true;
                case Sound.PlatformShake:
                    {
                        if (soundTimer.ContainsKey(sound))
                        {
                            float lastTimePLayed = soundTimer[sound];
                            float timerMax = 0.5f;
                            if (lastTimePLayed + timerMax < Time.time)
                            {
                                soundTimer[sound] = Time.time;
                                return true;
                            }
                            else
                                return false;
                        }
                        else
                            return true;
                    }
                    //break;
            }

        }
        else
        {
            return false;
        }
    }

    public static void PlaySound(Sound sound , Vector3 position)
    {
        if(CanPlaySound(sound))
        {
            Debug.Log("Creating sound!");
            GameObject soundObj = new GameObject("Sound");
            soundObj.transform.position = position;
            AudioSource audioSource = soundObj.AddComponent<AudioSource>();
            audioSource.clip = GetClip(sound);
            audioSource.Play();

            //GameObject.Destroy(soundObj, audioSource.clip.length);

        }
    }

    public static void PlaySound(Sound sound)
    {
        if (oneShotGameObject == null)
        {
            Debug.Log("Creating sound!");
            oneShotGameObject = new GameObject("Sound");
            oneShotAudioSource = oneShotGameObject.AddComponent<AudioSource>();

        }

        oneShotAudioSource.PlayOneShot(GetClip(sound));
    }


    public static AudioClip GetClip(Sound sound)
    {
        if(SoundManager.soundTimer != null)
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
