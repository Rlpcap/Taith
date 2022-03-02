using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BossCinematicPlayer : MonoBehaviour
{
    public GameObject portal;
    public PlayableDirector playableDirector;
    public Camera cutsceneCamera;
    public PlayableAsset bossCutscene;

    Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
        cutsceneCamera.enabled = false;
    }

    void Start()
    {
        if (GameManager.Instance.hasToPlayCinematic)
            PlayCutscene();
    }

    public void PlayCutscene()
    {
        GameManager.Instance.hasToPlayCinematic = false;
        PlayerModel.isLocked = true;
        CameraTarget.isLocked = true;
        _mainCamera.enabled = false;
        cutsceneCamera.enabled = true;

        playableDirector.Play(bossCutscene);
    }

    public void SwitchCameraToMain()
    {
        portal.SetActive(false);
        cutsceneCamera.enabled = false;
        _mainCamera.enabled = true;
        PlayerModel.isLocked = false;
        CameraTarget.isLocked = false;
    }
}
