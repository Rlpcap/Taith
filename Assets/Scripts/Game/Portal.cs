using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Portal : MonoBehaviour
{
    public string nextLevel;

    [ColorUsage(true, true)]
    public Color transitionColor;

    public bool lastLevel;

    public bool switchMusic;
    public int musicSongIndex;

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            if(lastLevel)
                GameManager.Instance.hasToPlayCinematic = true;

            //SceneManager.LoadScene(nextLevel);
            pl.OnPortalTrigger(transitionColor);
            StartCoroutine(WaitingLoadScene());
        }
    }

    IEnumerator WaitingLoadScene()
    {
        yield return UpdateManager.WaitForSecondsCustom(.8f);
        if (switchMusic) MusicManager.Instance.SwitchMusic(musicSongIndex);
        SceneManager.LoadScene(nextLevel);
    }
}
