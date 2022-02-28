using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    public GameObject prop;
    public GameObject prop2;
    public string triggerName;

    public void ActiveAnim()
    {
        prop.GetComponent<Animator>().SetTrigger(triggerName);
    }

    public void BookDown()
    {
        prop2.GetComponent<Animator>().SetBool("quest", true);
    }

    public void BookUp()
    {
        prop2.GetComponent<Animator>().SetBool("quest", false);
    }
}
