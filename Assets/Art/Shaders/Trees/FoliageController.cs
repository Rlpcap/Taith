using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageController : MonoBehaviour
{
    //Encontrar porque no puedo poner SetVector en listas de Materiales.
    public List<Material> foliage = new List<Material>();
    //public Material foliage;
    public Transform playerPos;

    // Update is called once per frame
    void Update()
    {
        foreach (var item in foliage)
        {
            item.SetVector("_PlayerPos", playerPos.position);
        }
    }
}
