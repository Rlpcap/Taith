using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    float speed;

    private void Start()
    {
        speed = Random.Range(0.3f, 0.6f);
    }

    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
    }
}
