//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 10 Apr 2022
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemActiveToggle : MonoBehaviour
{
    public GameObject subject;
    public bool defaultState = true;
    public GameObject syncStateObject;

    void Start()
    {
        if (subject == null)
            subject = gameObject;

        subject.SetActive(defaultState);
    }

    public void Toggle()
    {
        subject.SetActive(!subject.activeSelf);
    }

    private void FixedUpdate()
    {
        if (syncStateObject != null)
            subject.SetActive(syncStateObject.activeSelf);
    }
}
