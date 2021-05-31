using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicPlayer : MonoBehaviour
{
    public PlayableDirector playableDirector;
    public Camera cutsceneCamera;
    Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
        cutsceneCamera.enabled = false;
    }
    void Start()
    {
        if(GameManager.Instance.hasToPlayCinematic)
            PlayCutscene(GameManager.Instance.lastLevelAchieved);
    }

    public void PlayCutscene(int lastLevelAchieved)
    {
        Debug.Log("PLAYCINEMATIC");
        PlayerModel.isLocked = true;
        _mainCamera.enabled = false;
        cutsceneCamera.enabled = true;
        playableDirector.Play();
        GameManager.Instance.hasToPlayCinematic = false;
    }

    public void SwitchCameraToMain()
    {
        cutsceneCamera.enabled = false;
        _mainCamera.enabled = true;
        PlayerModel.isLocked = false;
    }
}


