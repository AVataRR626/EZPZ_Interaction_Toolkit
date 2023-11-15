//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 10 Apr 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemCycler : MonoBehaviour
{

    [Header("Item Management")]
    public int itemIndex = 0;
    public GameObject currentItem;
    public GameObject[] items;

    [Header("Event Management")]
    public bool loopCycle = true;
    public bool exclusiveMode = true;
    public bool cascadeMode = false;
    public UnityEvent onNextItem;
    public UnityEvent onPrevItem;
    public UnityEvent onChangeItem;
    public UnityEvent onFirstItem;
    public UnityEvent onLastItem;
    public UnityEvent onFirstOverflow;
    public UnityEvent onLastOverflow;
    public UnityEvent onOverflow;


    // Start is called before the first frame update
    void Start()
    {
        ActivateCurrentItem();
    }

    public void DisableAllItems()
    {
        foreach (GameObject g in items)
            g.SetActive(false);
    }

    public void ActivateCurrentItem()
    {
        itemIndex = Mathf.Clamp(itemIndex, 0, items.Length - 1);
        ActivateItem(itemIndex);
    }

    public void ActivateItem(int index)
    {
        itemIndex = index;

        if(exclusiveMode)
            DisableAllItems();

        if (cascadeMode)
        {
            
            DisableAllItems();

            Debug.Log("cascadeMode");

            for (int i = 0; i < index; i++)
            {
                if (items[i] != null)
                    items[i].SetActive(true);
            }
        }
    

        items[index].SetActive(true);
        currentItem = items[index];
    }

    public void NextItem()
    {
        itemIndex++;
        if (itemIndex >= items.Length)
        {
            if (loopCycle)
                itemIndex = 0;
            else
            {
                itemIndex = items.Length - 1;
                onLastOverflow.Invoke();
                onOverflow.Invoke();
            }
        }
        else if(itemIndex == items.Length - 1)
        {
            onLastItem.Invoke();
        }

        onNextItem.Invoke();
        onChangeItem.Invoke();

        ActivateCurrentItem();
    }

    public void PrevItem()
    {
        itemIndex--;
        if (itemIndex < 0)
        {
            if(loopCycle)
                itemIndex = items.Length - 1;
            else
            {
                itemIndex = 0;
                onFirstOverflow.Invoke();
                onOverflow.Invoke();
            }
        }
        else if(itemIndex == 0)
        {
            onFirstItem.Invoke();
        }


        onPrevItem.Invoke();
        onChangeItem.Invoke();

        ActivateCurrentItem();
    }

    public void SetExclusiveMode(bool newMode)
    {
        exclusiveMode = newMode;
        ActivateCurrentItem();
    }
}
