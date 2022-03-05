using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenLevelIntro : MonoBehaviour
{
    public Image loadingImage;

    void Start()
    {
        if(FindObjectOfType<GameManager>())
            StartCoroutine(AsyncLoadLevel(GameManager.Instance.loadingLevel));
        else
            StartCoroutine(AsyncLoadLevel("LevelIntro"));
    }

    IEnumerator AsyncLoadLevel(string levelToLoad)
    {
        yield return new WaitForSeconds(.5f);

        AsyncOperation asyncLoadLevel = SceneManager.LoadSceneAsync(levelToLoad);

        while (!asyncLoadLevel.isDone)
        {
            //loadingImage.fillAmount = asyncLoadLevel.progress;
            yield return null;
        }

        yield return new WaitForSeconds(.5f);
    }
}
