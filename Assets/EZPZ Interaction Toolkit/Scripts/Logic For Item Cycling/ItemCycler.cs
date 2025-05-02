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

    [Header("Synchronisation")]
    public bool autoSync = false;
    public NumberHolder indexSynchroniser;

    [Header("List Modes")]
    public bool loopCycle = true;
    public bool exclusiveMode = true;
    public bool cascadeMode = false;

    [Header("Event Management")]
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
        CullNulls();
        ActivateCurrentItem();
    }

    private void Update()
    {
        if (autoSync)
        {
            if (indexSynchroniser != null)
            {
                ActivateItem(indexSynchroniser.GetIntValue());
            }
        }

    }

    public void CullNulls()
    {
        List<GameObject> noNulls = new List<GameObject>();

        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null)
                noNulls.Add(items[i]);
        }

        items = noNulls.ToArray();
    }

    public void DisableAllItems()
    {
        foreach (GameObject g in items)
            g.SetActive(false);
    }

    public void AutoSync(bool newMode)
    {
        autoSync = newMode;
    }

    public void ActivateCurrentItem()
    {
        itemIndex = Mathf.Clamp(itemIndex, 0, items.Length - 1);
        ActivateItem(itemIndex);
    }

    public void SelectItem(int index)
    {
        ActivateItem(index);
    }

    public void SelectItemRandom()
    {
        ActivateItemRandom();
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

    public void ActivateItemRandom()
    {
        int rSelect = itemIndex;
            
        while(rSelect == itemIndex)
            rSelect = Random.Range(0, items.Length);

        ActivateItem(rSelect);
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

        HandleSkip(true);

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

        HandleSkip(false);

        onPrevItem.Invoke();
        onChangeItem.Invoke();

        ActivateCurrentItem();
    }

    public void HandleSkip(bool next)
    {
        CycledItem ci = items[itemIndex].GetComponent<CycledItem>();

        if (ci != null)
        {
            if (ci.skip)
            {
                Debug.Log("HandleSkip: " + ci.name);
                if (next)
                {
                    SkipNext(1);
                }
                else
                {
                    SkipPrev(1);
                }
                return;
            }
        }
    }

    public void SkipNext(int level)
    {
        if (level < items.Length)
        {
            int nextIndex = itemIndex + 1;

            if(nextIndex > items.Length - 1)
            {
                //handle overflow case
                if(loopCycle)
                {
                    nextIndex = 0;
                }
            }

            itemIndex = nextIndex;

            CycledItem ci = items[nextIndex].GetComponent<CycledItem>();

            if(ci != null)
            {
                Debug.Log("SkipNext: " + ci.name + " | " + level);
                if (ci.skip)
                {
                    SkipNext(level + 1);
                }
            }
        }
    }

    public void SkipPrev(int level)
    {
        if (level < items.Length)
        {
            int nextIndex = itemIndex - 1;

            if (nextIndex < 0)
            {
                //handle overflow case
                if (loopCycle)
                {
                    nextIndex = items.Length - 1;
                }
            }

            itemIndex = nextIndex;

            CycledItem ci = items[nextIndex].GetComponent<CycledItem>();

            if (ci != null)
            {
                Debug.Log("SkipNext: " + ci.name + " | " + level);
                if (ci.skip)
                {
                    SkipPrev(level + 1);
                }
            }
        }
    }

    public void SetExclusiveMode(bool newMode)
    {
        exclusiveMode = newMode;
        ActivateCurrentItem();
    }
}
