//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 10 Apr 2022
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKill : MonoBehaviour
{
    public float timeToLive = 5;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (timeToLive > 0)
            timeToLive -= Time.fixedDeltaTime;
        else
            Destroy(gameObject);
    }
}
