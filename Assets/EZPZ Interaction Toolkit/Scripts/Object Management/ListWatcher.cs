//EZPZ Interaction Toolkit
//by Matt Cabanag
//created: some time early 2024??

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ListWatcher : MonoBehaviour
{
    public float checkFrequency;
    public bool invokeOnce;
    public List<GameObject> watchList;

    [Header("Event Handling")]
    public UnityEvent onEmpty;
    public UnityEvent onFull;
    public UnityEvent onCountMatch;
    public bool invokedFlag = false;
    public int activeCount = 0;
    // Start is called before the first frame update

    private void Start()
    {
        CheckContinuous();
    }

    public void CheckContinuous()
    {
        CheckIfEmpty();
        CheckIfFull();

        if (gameObject.activeSelf)
            Invoke("CheckContinuous", checkFrequency);
    }

    public void CountActives()
    {
        int i = 0;
        activeCount = 0;
        while (i < watchList.Count)
        {
            if (watchList[i] != null)
            {
                if (watchList[i].activeSelf)
                    activeCount++;
            }
            i++;
        }
    }

    public bool CheckIfEmpty()
    {
        if (invokeOnce && invokedFlag)
            return false;

        CountActives();

        if (activeCount == 0)
        {
            if (invokeOnce)
            {
                if (!invokedFlag)
                    invokedFlag = true;
            }

            onEmpty.Invoke();
            return true;
        }

        return false;
    }

    public bool CheckIfFull()
    {
        if (invokeOnce && invokedFlag)
            return false;

        CountActives();

        if (activeCount == watchList.Count)
        {
            if (invokeOnce)
            {
                if (!invokedFlag)
                    invokedFlag = true;
            }

            onFull.Invoke();
            return true;
        }

        return false;
    }

    public bool CheckMatchCount(int test)
    {
        if (invokeOnce && invokedFlag)
            return false;

        CountActives();

        if (activeCount == test)
        {
            if (invokeOnce)
            {
                if (!invokedFlag)
                    invokedFlag = true;
            }

            onCountMatch.Invoke();
            return true;
        }

        return false;
    }

    
}
