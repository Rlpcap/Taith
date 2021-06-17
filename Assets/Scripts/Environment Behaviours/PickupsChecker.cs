using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupsChecker : MonoBehaviour
{
    public PickupObject[] listOfPickups;

    void Start()
    {
        listOfPickups = Object.FindObjectsOfType<PickupObject>();
        GameManager.Instance.HideObjects(listOfPickups);
    }

}
