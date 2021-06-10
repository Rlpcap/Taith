using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    protected bool _interacting;

    public abstract void Interact();

    protected abstract void StartInteraction();

    protected abstract void EndInteraction();
}
