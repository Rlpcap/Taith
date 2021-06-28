using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Portal : MonoBehaviour
{
    public string nextLevel;

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
            pl.OnPortalTrigger();
            StartCoroutine(WaitingLoadScene());
        }
    }

    IEnumerator WaitingLoadScene()
    {
        yield return UpdateManager.WaitForSecondsCustom(1.5f);
        if (switchMusic) MusicManager.Instance.SwitchMusic(musicSongIndex);
        SceneManager.LoadScene(nextLevel);
    }
}
