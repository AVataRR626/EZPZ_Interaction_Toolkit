using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMatchRelay : MonoBehaviour
{    
    public List<TextMatchTrigger> relayList;
    public bool autoPopulateChildren = true;

    private void Start()
    {
        if(autoPopulateChildren)
        {
            foreach(Transform child in transform)
            {
                TextMatchTrigger tmt = child.GetComponent<TextMatchTrigger>();

                if (tmt != null)
                    relayList.Add(tmt);
            }
        }
    }

    public void CheckMatch()
    {
        foreach (TextMatchTrigger t in relayList)
            t.CheckMatch();
    }
}
