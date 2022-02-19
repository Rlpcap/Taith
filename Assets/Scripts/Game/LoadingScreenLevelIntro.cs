using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenLevelIntro : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(AsyncLoadLevelIntro());
    }

    IEnumerator AsyncLoadLevelIntro()
    {
        yield return new WaitForSeconds(.5f);

        AsyncOperation asyncLoadIntro = SceneManager.LoadSceneAsync("LevelIntro");
        while (!asyncLoadIntro.isDone)
        {
            Debug.Log(asyncLoadIntro.progress);
            yield return null;
        }

        yield return new WaitForSeconds(.5f);
    }
}
