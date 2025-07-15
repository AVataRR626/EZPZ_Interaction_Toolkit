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
    public Vector3 randomPositionOffset;

    private void Start()
    {
        if (spawnPoint == null)
            spawnPoint = transform;
    }

    public void Spawn()
    {

        Vector3 offset = new Vector3(
            Random.Range(-randomPositionOffset.x, randomPositionOffset.x),
            Random.Range(-randomPositionOffset.y, randomPositionOffset.y),
            Random.Range(-randomPositionOffset.y, randomPositionOffset.y)
            );

        lastObject = Instantiate(template, spawnPoint.position + offset, spawnPoint.rotation);
    }
}
