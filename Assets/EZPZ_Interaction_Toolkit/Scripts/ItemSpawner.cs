//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 11 Jul 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject prefab;
    public Transform spawnPoint;

    private void Start()
    {
        if (spawnPoint == null)
            spawnPoint = transform;
    }

    public void Spawn()
    {
        Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
    }
}
