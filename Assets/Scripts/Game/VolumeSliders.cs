using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeSliders : MonoBehaviour
{
    public AudioMixer audioMixer;

    public void SetMasterVolume(float vol)
    {
        audioMixer.SetFloat("MasterVol", vol);
    }

    public void SetMusicVolume(float vol)
    {
        audioMixer.SetFloat("MusicVol", vol);
    }

    public void SetSoundEffectsVolume(float vol)
    {
        audioMixer.SetFloat("SoundEffectsVol", vol);
    }

    public void SetEnvironmentVolume(float vol)
    {
        audioMixer.SetFloat("EnvironmentVol", vol);
    }
}
