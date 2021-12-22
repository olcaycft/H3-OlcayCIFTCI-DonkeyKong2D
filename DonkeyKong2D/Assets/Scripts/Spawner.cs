using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject prefab;
    private float minTime = 2f;
    private float maxTime = 4f;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        Instantiate(prefab, transform.position, Quaternion.identity);
        Invoke(nameof(Spawn),UnityEngine.Random.Range(minTime,maxTime)); //continuously will call itself in a range of min and max time
    }
}
