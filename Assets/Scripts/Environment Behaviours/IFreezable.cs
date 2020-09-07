using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFreezable
{
    void Freeze();
    void Unfreeze(bool wf);
    IEnumerator FreezeTime(float f);
}
