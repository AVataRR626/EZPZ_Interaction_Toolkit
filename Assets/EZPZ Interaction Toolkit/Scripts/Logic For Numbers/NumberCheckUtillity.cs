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
        if (comparison == reference.value)
        {
            onEqual.Invoke();
        }
        else if (comparison > reference.value)
        {
            onGreaterThan.Invoke();
        }
        else if (comparison < reference.value)
        {
            onLessThan.Invoke();
        }
        else if (comparison >= reference.value)
        {
            onGreaterOrEqual.Invoke();
        }
        else if (comparison <= reference.value)
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
