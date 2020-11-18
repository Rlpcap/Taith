using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject fade;

    void Start()
    {
        StartCoroutine(OutAnim());
    }

    IEnumerator OutAnim()
    {
        yield return new WaitForSeconds(1.5f);
        fade.SetActive(false);
    }

    public void BtnStartGame()
    {
        SceneManager.LoadScene(2);
    }

    public void BtnExit()
    {
        Application.Quit();
    }
}
