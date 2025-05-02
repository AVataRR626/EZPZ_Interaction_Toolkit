//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 27 Jan 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class NumberCheckUtillity : MonoBehaviour
{
    [Header("Number Holder References")]
    public NumberHolder reference;
    public List<NumberHolder> referencePeers;

    [Header("Purchase Logic Events")]
    public UnityEvent onEnoughToBuy;
    public UnityEvent onNotEnoughToBuy;

    [Header("Generic Logic Events")]
    public UnityEvent onEqual;
    public UnityEvent onGreaterThan;
    public UnityEvent onLessThan;    
    public UnityEvent onGreaterOrEqual;
    public UnityEvent onLessOrEqual;

    [Header("Peer Comparisons")]
    public UnityEvent onGreatest;
    public UnityEvent onNotGreatest;
    public UnityEvent onLeast;
    public UnityEvent onNotLeast;

    public void CheckNumber(float comparison)
    {
        Debug.Log("CheckNumber: i:" + comparison + " VS r:" + reference.value);

        if (comparison == reference.value)
        {
            onEqual.Invoke();
        }

        if (reference.value > comparison)
        {
            onGreaterThan.Invoke();
        }

        if (reference.value < comparison)
        {
            onLessThan.Invoke();
        }

        if (reference.value >= comparison)
        {
            onGreaterOrEqual.Invoke();
        }

        if (reference.value <= comparison)
        {
            onLessOrEqual.Invoke();
        }
    }

    public void CheckPeers()
    {
        float greatest = reference.value;
        float least = reference.value;

        for (int i = 0; i < referencePeers.Count; i++)
        {
            if (greatest < referencePeers[i].value)
            {
                greatest = referencePeers[i].value;
            }

            if(least > referencePeers[i].value )
            {
                least = referencePeers[i].value;
            }
        }

        if (reference.value == greatest)
        {
            onGreatest.Invoke();
        }
        else
        {
            onNotGreatest.Invoke();
        }

        if(reference.value == least)
        {
            onLeast.Invoke();
        }
        else
        {
            onNotLeast.Invoke();
        }

        CheckPeer();
    }

    public void CheckPeer()
    {
        if(referencePeers.Count == 1)
        {
            CheckNumber(referencePeers[0].value);
        }
    }
    
    public void Buy(float price)
    {
        if(reference.value >= price)
        {
            reference.value -= price;
            onEnoughToBuy.Invoke();
        }
        else
        {
            onNotEnoughToBuy.Invoke();
        }
    }

}
