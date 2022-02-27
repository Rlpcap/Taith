using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        MusicManager.Instance.SwitchMusic(0);
        if (!GameManager.Instance.bossTime)
            GameManager.Instance.loadingLevel = "LevelIntro";
        else
            GameManager.Instance.loadingLevel = "BossLevel";
        SceneManager.LoadScene("LoadingScreen");
    }
}
