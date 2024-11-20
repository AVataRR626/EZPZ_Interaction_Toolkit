using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ListWatcher : MonoBehaviour
{
    public float checkFrequency;
    public List<GameObject> watchList;

    [Header("Event Handling")]
    public UnityEvent onEmpty;
    public UnityEvent onCountMatch;
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

    public void CheckIfEmpty()
    {
        int i = 0;
        while(i < watchList.Count)
        {
            if (watchList[i] == null)
            {
                watchList.RemoveAt(i);
            }
            else
            {
                i++;
            }
        }


        if (watchList.Count == 0)
        {
            onEmpty.Invoke();
        }
    }

    public void CheckMatchCount(int test)
    {
        if(watchList.Count == test)
        {
            onCountMatch.Invoke();
        }
    }
}
