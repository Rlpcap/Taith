using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSliders : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume(float vol)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(vol) * 20);
        Debug.Log(Mathf.Log10(vol) * 20);
    }

    public void SetMusicVolume(float vol)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(vol) * 20);
    }

    public void SetSoundEffectsVolume(float vol)
    {
        audioMixer.SetFloat("SoundEffectsVol", Mathf.Log10(vol) * 20);
    }

    public void SetEnvironmentVolume(float vol)
    {
        audioMixer.SetFloat("EnvironmentVol", Mathf.Log10(vol) * 20);
    }
}
