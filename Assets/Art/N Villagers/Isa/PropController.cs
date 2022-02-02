using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    public GameObject prop;
    public string triggerName;

    public void ActiveAnim()
    {
        prop.GetComponent<Animator>().SetTrigger(triggerName);
    }
}
