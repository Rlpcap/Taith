using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    public List<AudioClip> allMusics;

    AudioSource _AS;

    void Start()
    {
        _AS = GetComponent<AudioSource>();
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
}
