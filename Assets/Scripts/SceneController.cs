using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    public GameObject fade;


    void Start()
    {
        StartCoroutine(OutAnim());
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator OutAnim()
    {
        yield return new WaitForSeconds(1.5f);
        fade.SetActive(false);
    }
}
