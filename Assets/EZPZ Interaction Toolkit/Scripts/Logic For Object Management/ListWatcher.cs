using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ListWatcher : MonoBehaviour
{
    public float checkFrequency;
    public bool invokeOnce;
    public List<GameObject> watchList;

    [Header("Event Handling")]
    public UnityEvent onEmpty;
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

        if(gameObject.activeSelf)
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

    public void CheckIfEmpty()
    {
        if (invokeOnce && invokedFlag)
            return;


        CountActives();

        if (activeCount == 0)
        {

            if (invokeOnce)
            {
                if (!invokedFlag)
                    invokedFlag = true;
            }

            onEmpty.Invoke();
        }
    }

    public void CheckMatchCount(int test)
    {

        if (invokeOnce && invokedFlag)
            return;

        CountActives();

        if (activeCount == test)
        {

            if (invokeOnce)
            {
                if (!invokedFlag)
                    invokedFlag = true;
            }

            onCountMatch.Invoke();
        }
    }
}
