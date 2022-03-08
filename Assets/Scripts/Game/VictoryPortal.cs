using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class VictoryPortal : MonoBehaviour
{
    public string nextLevel;

    [ColorUsage(true, true)]
    public Color transitionColor;

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
        GameManager.Instance.loadingLevel = nextLevel;
        SceneManager.LoadScene("Menu");
    }

}
