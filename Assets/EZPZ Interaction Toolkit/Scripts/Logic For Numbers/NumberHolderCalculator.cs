//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 13 Jul 2025

using UnityEngine;
using UnityEngine.Events;

//binary operator for the NumberHolder class!
//chose "Calculator" instead of "Operator" because non programmers :p
public class NumberHolderCalculator : MonoBehaviour
{
    [Header("Basic Links")]
    public NumberHolder operandA;
    public NumberHolder operandB;
    public NumberHolder result;

    [Header("Events")]    
    public UnityEvent onAdd;
    public UnityEvent onSubtract;
    public UnityEvent onMultiply;
    public UnityEvent onDivide;
    public UnityEvent onDivideByZero;

    public void Add()
    {
        result.value = operandA.value + operandB.value;
        onAdd.Invoke();
    }

    public void Subtract()
    {
        result.value = operandA.value - operandB.value;
        onSubtract.Invoke();
    }

    public void Multiply()
    {
        result.value = operandA.value * operandB.value;
        onMultiply.Invoke();
    }

    public void Divide()
    {
        if (operandB.value != 0)
        {
            result.value = operandA.value / operandB.value;
            onDivide.Invoke();
        }
        else
        {
            onDivideByZero.Invoke();
            Debug.LogError("CANT DIVIDE BY ZERO! " + name);
        }
    }
}
