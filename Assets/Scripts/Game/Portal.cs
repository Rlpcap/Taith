using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class Portal : MonoBehaviour
{
    public string nextLevel;

    [ColorUsage(true, true)]
    public Color transitionColor;

    public bool switchMusic;
    public int musicSongIndex;

    public int ID;

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            //SceneManager.LoadScene(nextLevel);
            GetComponents<Collider>().ToList().Where(x => x.isTrigger).First().enabled = false;
            pl.OnPortalTrigger(transitionColor);
            QuestManager.Instance.SaveQuests();
            UpdateData.Instance.SaveNPCData();
            StartCoroutine(WaitingLoadScene());
        }
    }

    IEnumerator WaitingLoadScene()
    {
        yield return UpdateManager.WaitForSecondsCustom(.8f);
        if (switchMusic) MusicManager.Instance.SwitchMusic(musicSongIndex);
        GameManager.Instance.loadingLevel = nextLevel;
        SceneManager.LoadScene("LoadingScreen");
    }
}
