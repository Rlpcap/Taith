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
    }
}
