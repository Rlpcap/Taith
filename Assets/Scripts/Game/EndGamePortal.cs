using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class EndGamePortal : MonoBehaviour
{
    public int ID;
    [ColorUsage(true, true)]
    public Color transitionColor;

    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            //Completar un goal de la quest previa al boss level
            GetComponents<Collider>().ToList().Where(x => x.isTrigger).First().enabled = false;
            GameManager.Instance.CompleteEndPortal(ID);
            pl.OnPortalTrigger(transitionColor);
            QuestManager.Instance.SaveQuests();
            UpdateData.Instance.SaveNPCData();
            StartCoroutine(WaitingLoadScene());
        }
    }

    IEnumerator WaitingLoadScene()
    {
        yield return UpdateManager.WaitForSecondsCustom(.8f);
        if (!GameManager.Instance.bossTime)
        {
            MusicManager.Instance.SwitchMusic(0);
            GameManager.Instance.loadingLevel = "LevelIntro";
        }
        else
        {
            MusicManager.Instance.SwitchMusic(2);
            GameManager.Instance.loadingLevel = "BossLevel";
        }
        SceneManager.LoadScene("LoadingScreen");
    }
}
