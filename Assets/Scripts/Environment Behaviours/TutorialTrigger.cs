using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject animatedObj;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerModel>())
        {
            animatedObj.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerModel>())
        {
            animatedObj.SetActive(false);
        }
    }
}
