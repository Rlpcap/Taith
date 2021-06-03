using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorruptionController : MonoBehaviour
{
    public float radius;

    public List<FallingFloor> corruptedFloors = new List<FallingFloor>();

    void Start()
    {
        foreach (var floor in corruptedFloors)
        {
            floor.SetDissolveRadius(radius).SetEnemyPos(transform.position);
            floor.CorruptionOn();
        }
    }

    public void CorruptionControl()
    {
        radius = 0;
        foreach (var floor in corruptedFloors)
        {
            floor.CorruptionOff();
        }
    }
}
