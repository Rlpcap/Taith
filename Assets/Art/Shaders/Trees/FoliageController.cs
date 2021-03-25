using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoliageController : MonoBehaviour
{
    //Encontrar porque no puedo poner SetVector en listas de Materiales.
    //public List<Material> foliage = new List<Material>();
    public Material foliage;
    public Transform playerPos;

    // Update is called once per frame
    void Update()
    {
        foliage.SetVector("_PlayerPos", playerPos.position);
        //for (int i = 0; i < foliage.Count; i++)
        //{
        //    //foliage.SetVector("_PlayerPos", playerPos.position);
        //}
    }
}
