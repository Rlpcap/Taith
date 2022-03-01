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
        EnemyHit,
        EnemyDeath,
        PineHit,
        PumpkinBounce1,
        PumpkinBounce2,
        PumpkinBounce3,
        GrassHit,
        TreeMove,
        DummyHit,
        PageFlip,
        PlayerLanding,
        scrollPickup,
        waterSplash,
        MudStep,
        FireDash,
        BossFireBall,
        FreezePower,
        WindPower,
        TimePower,
        IceHit,
        WallBreak
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
                            float timerMax = 1f;
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

                case Sound.MudStep:
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

            }


        }
        else
        {
            return false;
        }
    }

    public static void PlaySound(Sound sound, Vector3 position)
    {
        if (CanPlaySound(sound))
        {

            //GameObject soundObj = new GameObject("Sound");
            var obj = SoundSpawner.instance.pool.GetObject();
            obj.transform.position = position;


            //Ajustamos las propiedades del audio source aca --v

            obj.audioSource.clip = GetClip(sound);
            obj.audioSource.spatialBlend = 0.8f;
            obj.audioSource.Play();

            //GameObject.Destroy(soundObj, audioSource.clip.length);

        }
    }

    public static void PlaySound(Sound sound)
    {
        if (oneShotGameObject == null)
        {
            // oneShotGameObject = new GameObject("Sound");
            var obj = SoundSpawner.instance.pool.GetObject();
            oneShotAudioSource = obj.audioSource;
            oneShotAudioSource.spatialBlend = 0.0f;
        }


        oneShotAudioSource.PlayOneShot(GetClip(sound));
    }

    public static void PlayRandom(List<Sound> sounds)
    {
        if (oneShotGameObject == null)
        {
            // oneShotGameObject = new GameObject("Sound");
            var obj = SoundSpawner.instance.pool.GetObject();
            oneShotAudioSource = obj.audioSource;
            oneShotAudioSource.spatialBlend = 0.0f;

        }

        var index = Random.Range(0, sounds.Count);

        oneShotAudioSource.PlayOneShot(GetClip(sounds[index]));

    }

    public static void PlayRandom(List<Sound> sounds, Vector3 position)
    {
        var index = Random.Range(0, sounds.Count);

        if (CanPlaySound(sounds[index]))
        {
            Debug.Log("Creating sound!");
            //GameObject soundObj = new GameObject("Sound");
            var obj = SoundSpawner.instance.pool.GetObject();
            obj.transform.position = position;


            //Ajustamos las propiedades del audio source aca --v

            obj.audioSource.clip = GetClip(sounds[index]);
            obj.audioSource.spatialBlend = 0.8f;
            obj.audioSource.Play();

            //GameObject.Destroy(soundObj, audioSource.clip.length);

        }

    }


    public static AudioClip GetClip(Sound sound)
    {
        if (SoundManager.soundTimer != null)
            foreach (GameManager.SoundAudioClip soundAudioClip in GameManager.Instance.soundClips)
            {
                if (soundAudioClip.sound == sound)
                {
                    return soundAudioClip.audioClip;
                }

            }
        return null;
    }
}
