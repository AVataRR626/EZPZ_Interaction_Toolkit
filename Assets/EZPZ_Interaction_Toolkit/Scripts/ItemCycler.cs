//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 10 Apr 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCycler : MonoBehaviour
{
    public int itemIndex = 0;
    public GameObject[] items;

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

    public void ActivateItem(int i)
    {
        DisableAllItems();
        items[i].SetActive(true);
    }

    public void NextItem()
    {
        itemIndex++;
        if (itemIndex >= items.Length)
            itemIndex = 0;

        ActivateCurrentItem();
    }

    public void PrevItem()
    {
        itemIndex--;
        if (itemIndex < 0)
            itemIndex = items.Length - 1;

        ActivateCurrentItem();
    }
}
