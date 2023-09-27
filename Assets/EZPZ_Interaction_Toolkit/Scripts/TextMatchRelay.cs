using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextMatchRelay : MonoBehaviour
{    
    public List<TextMatchTrigger> relayList;

    public void CheckMatch()
    {
        foreach (TextMatchTrigger t in relayList)
            t.CheckMatch();
    }
}
