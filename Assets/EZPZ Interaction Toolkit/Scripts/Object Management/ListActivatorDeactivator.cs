//EZPZ Interaction Toolkit
//by Matt Cabanag
//created: 16 Jul 2025

using System.Collections.Generic;
using UnityEngine;

public class ListActivatorDeactivator : MonoBehaviour
{
    public List<GameObject> objectList;

    public void ActivateAll()
    {        
        ActivateAll(true);
    }

    public void DeactivateAll()
    {
        ActivateAll(false);
    }

    public void ActivateAll(bool mode)
    {
        foreach (GameObject o in objectList)
            o.SetActive(mode);
    }
}
