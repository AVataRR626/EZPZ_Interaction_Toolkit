//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 27 Jan 2024

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class NumberCheckUtillity : MonoBehaviour
{
    public NumberHolder reference;

    [Header("Purchase Logic Events")]
    public UnityEvent onEnoughToBuy;
    public UnityEvent onNotEnoughToBuy;

    [Header("Generic Logic Events")]
    public UnityEvent onEqual;
    public UnityEvent onGreaterThan;
    public UnityEvent onLessThan;    
    public UnityEvent onGreaterOrEqual;
    public UnityEvent onLessOrEqual;    

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
