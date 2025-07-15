//EZPZ Interaction Toolkit
//Matt Cabanag 9 Apr
//
//Companion class for ItemCycler...
//
//Add to this an item if you want it to be skippable
//Set skip to true

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycledItem : MonoBehaviour
{
    public bool skip = false;

    public void SetSkip(bool newSetting)
    {
        skip = newSetting;
    }
}
