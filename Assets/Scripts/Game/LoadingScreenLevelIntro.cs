using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenLevelIntro : MonoBehaviour
{
    void Start()
    {
        if(GameManager.Instance != null)
            StartCoroutine(AsyncLoadLevel(GameManager.Instance.loadingLevel));
        else
            StartCoroutine(AsyncLoadLevel("LevelIntro"));
    }

    IEnumerator AsyncLoadLevel(string levelToLoad)
    {
        yield return new WaitForSeconds(.5f);

        AsyncOperation asyncLoadIntro = SceneManager.LoadSceneAsync(levelToLoad);

        while (!asyncLoadIntro.isDone)
        {
            Debug.Log(asyncLoadIntro.progress);
            yield return null;
        }

        yield return new WaitForSeconds(.5f);
    }
}
