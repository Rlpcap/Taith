using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject pauseScreen, optionsScreen;

    public void BtnBackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    public void BtnOpenOptions()
    {
        optionsScreen.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void BtnCloseOptions()
    {
        pauseScreen.SetActive(true);
        optionsScreen.SetActive(false);
    }
}
