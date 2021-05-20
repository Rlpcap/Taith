using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            GetComponent<BoxCollider>().enabled = false;
        }
    }

    IEnumerator FallingBlocks()
    {
        var index = 0;
        while(index < floorTimers.Count)
        {
            yield return new WaitForSeconds(floorTimers[index]);

            var floorsToFall = fallingFloors.Where(f => f.ID == index);
            foreach (var floor in floorsToFall)
            {
                StartCoroutine(floor.StartFalling());
            }

            //foreach (var floor in fallingFloors)
            //{
            //    if (floor.ID == index)
            //    {
            //        StartCoroutine(floor.StartFalling());
            //    }
            //}

            index++;

            yield return null;
        }
        foreach (var floor in fallingFloors)
        {
            floor.CanBeDestroy = true;
        }
    }
}
