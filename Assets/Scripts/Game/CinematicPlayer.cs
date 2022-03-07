using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CinematicPlayer : MonoBehaviour
{
    public PlayerModel pl;
    public PlayableDirector playableDirector;
    public Camera cutsceneCamera;

    public List<PlayableAsset> listOfCutscenes = new List<PlayableAsset>();

    public List<CorruptionController> corruptionConstrollers = new List<CorruptionController>();

    Camera _mainCamera;

    void Awake()
    {
        _mainCamera = Camera.main;
        cutsceneCamera.enabled = false;
    }
    void Start()
    {
        if (GameManager.Instance.hasToPlayCinematic)
            PlayCutscene(GameManager.Instance.lastLevelAchieved);
    }

    public void PlayCutscene(int lastLevelAchieved)
    {
        Debug.Log("PLAYCINEMATIC");
        corruptionConstrollers[lastLevelAchieved - 1].CorruptFloors();
        PlayerModel.isLocked = true;
        CameraTarget.isLocked = true;
        _mainCamera.enabled = false;
        cutsceneCamera.enabled = true;
        //playableDirector.Play(listOfCutscenes[lastLevelAchieved], DirectorWrapMode.None);
        if (listOfCutscenes[lastLevelAchieved - 1] != null)
            playableDirector.Play(listOfCutscenes[lastLevelAchieved - 1], DirectorWrapMode.None);

        GameManager.Instance.hasToPlayCinematic = false;
    }

    public void PlayCutscene(PlayableAsset cinematic)
    {
        //pl.CanMove = false;
        pl.Move(0, 0);
        PlayerModel.isLocked = true;
        CameraTarget.isLocked = true;
        _mainCamera.enabled = false;
        cutsceneCamera.enabled = true;
        playableDirector.Play(cinematic, DirectorWrapMode.None);
    }

    public void SwitchCameraToMain()
    {
        cutsceneCamera.enabled = false;
        _mainCamera.enabled = true;
        PlayerModel.isLocked = false;
        CameraTarget.isLocked = false;
        pl.CanMove = true;
    }
}


