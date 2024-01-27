//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 11 Jul 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject template;
    public Transform spawnPoint;
    public GameObject lastObject;

    private void Start()
    {
        if (spawnPoint == null)
            spawnPoint = transform;
    }

    public void Spawn()
    {
        lastObject = Instantiate(template, spawnPoint.position, spawnPoint.rotation);
    }
}
