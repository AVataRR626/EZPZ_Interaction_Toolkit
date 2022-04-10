//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 8 Apr 2022
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnparentOnStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.parent = null;        
    }

}
