using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLevel : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);

        if (Input.GetKeyDown(KeyCode.B))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

        if (Input.GetKeyDown(KeyCode.N))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        if (Input.GetKeyDown(KeyCode.Alpha5))
            SceneManager.LoadScene("Level5");

    }
}
