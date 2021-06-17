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
        AsyncOperation asyncLoadIntro = SceneManager.LoadSceneAsync("LevelIntro");
        while (asyncLoadIntro.progress < 1)
        {
            Debug.Log(asyncLoadIntro.progress);
            yield return new WaitForEndOfFrame();
        }
    }
}
