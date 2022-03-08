using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider masterVol, musicVol, sfxVol, environmentVol;

    private void Start()
    {
        float sliderValue;
        audioMixer.GetFloat("MasterVol", out sliderValue);
        masterVol.value = Mathf.Pow(10, sliderValue / 20);

        audioMixer.GetFloat("MusicVol", out sliderValue);
        musicVol.value = Mathf.Pow(10, sliderValue / 20);

        audioMixer.GetFloat("SoundEffectsVol", out sliderValue);
        sfxVol.value = Mathf.Pow(10, sliderValue / 20);

        audioMixer.GetFloat("EnvironmentVol", out sliderValue);
        environmentVol.value = Mathf.Pow(10, sliderValue / 20);
    }

    public void SetMasterVolume(float vol)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(vol) * 20);
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
