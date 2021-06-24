using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Portal : MonoBehaviour
{
    public string nextLevel;

    public bool lastLevel;
    private void OnTriggerEnter(Collider coll)
    {
        var pl = coll.GetComponent<PlayerModel>();
        if (pl)
        {
            if(lastLevel)
                GameManager.Instance.hasToPlayCinematic = true;

            SceneManager.LoadScene(nextLevel);
            //pl.OnPortalTrigger();
            //StartCoroutine(WaitingLoadScene());
        }
    }

    IEnumerator WaitingLoadScene()
    {
        yield return UpdateManager.WaitForSecondsCustom(1f);
        SceneManager.LoadScene(nextLevel);
    }
}
