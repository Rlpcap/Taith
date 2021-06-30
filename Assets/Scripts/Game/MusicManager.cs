using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>, IPause
{
    public List<AudioClip> allMusics;

    AudioSource _AS;

    float _audioSourceVol;

    void Start()
    {
        _AS = GetComponent<AudioSource>();
        UpdateManager.Instance.AddElementPausable(this);
        _audioSourceVol = _AS.volume;
    }

    public void SwitchMusic(SoundManager.Sound music)
    {
        _AS.clip = GameManager.Instance.soundClips[0].audioClip;
        _AS.Play();
    }

    public void SwitchMusic(int index)
    {
        _AS.clip = allMusics[index];
        _AS.Play();
    }

    public void OnPause()
    {
        //_AS.volume /= 4;
    }

    public void OnUnpause()
    {
        //_AS.volume = _audioSourceVol;
    }
}
