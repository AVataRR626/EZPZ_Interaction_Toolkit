using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckIsActive : MonoBehaviour
{
    public GameObject checkItem;
    public UnityEvent onCheckTrue;
    public UnityEvent onCheckFalse;

    public void DoCheck()
    {
        if (checkItem.activeSelf)
            onCheckTrue.Invoke();
        else
            onCheckFalse.Invoke();
    }
}
