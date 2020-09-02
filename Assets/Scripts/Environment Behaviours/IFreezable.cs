using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFreezable
{
    void Freeze();
    void Unfreeze();
    IEnumerator FreezeTime(float f);
}
