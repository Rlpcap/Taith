using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    void Update()
    {
         CheckButtonPress();
    }

    public void CheckButtonPress()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SceneManager.LoadScene(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            SceneManager.LoadScene(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            SceneManager.LoadScene(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            SceneManager.LoadScene(3);
        else if (Input.GetKeyDown(KeyCode.Alpha5))
            SceneManager.LoadScene(4);
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            SceneManager.LoadScene(5);
        else if (Input.GetKeyDown(KeyCode.Alpha7))
            SceneManager.LoadScene(6);
        else if (Input.GetKeyDown(KeyCode.Alpha8))
            SceneManager.LoadScene(7);
        else if (Input.GetKeyDown(KeyCode.Alpha9))
            SceneManager.LoadScene(15);
        else if (Input.GetKeyDown(KeyCode.Alpha0))
            SceneManager.LoadScene(16);
    }
}
