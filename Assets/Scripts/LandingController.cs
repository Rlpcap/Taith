using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LandingController : MonoBehaviour
{
    public GameObject fade;
    bool _pressedKey = false;

    void Update()
    {
        if (Input.anyKeyDown && !_pressedKey)
        {
            StartCoroutine(OutAnim());
            _pressedKey = true;
        }
    }

    IEnumerator OutAnim()
    {
        fade.GetComponent<Animator>().SetTrigger("out");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene("Menu");
    }
}
