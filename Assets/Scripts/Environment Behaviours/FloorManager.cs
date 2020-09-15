using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public List<FallingFloor> fallingFloors = new List<FallingFloor>();

    public List<float> floorTimers = new List<float>();

    private void OnTriggerEnter(Collider coll)
    {
        if(coll.gameObject.layer == 8)
        {
            StartCoroutine(FallingBlocks());
        }
    }

    IEnumerator FallingBlocks()
    {
        var index = 0;
        while(index < floorTimers.Count)
        {
            yield return new WaitForSeconds(floorTimers[index]);

            foreach (var floor in fallingFloors)
            {
                if (floor.ID == index)
                {
                    StartCoroutine(floor.StartFalling());
                }
            }
            index++;

            yield return null;
        }
        foreach (var floor in fallingFloors)
        {
            floor.CanBeDestroy = true;
        }
    }
}
