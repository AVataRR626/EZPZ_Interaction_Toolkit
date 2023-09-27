using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextMatchTrigger : MonoBehaviour
{
    public string matchText;
    public TextMeshProUGUI textReference;
    public bool strictMatch = false;
    public UnityEvent onTextMatch;

    public void CheckMatch()
    {
        if(strictMatch)
        {
            if (textReference.text.Equals(matchText))
                onTextMatch.Invoke();
        }
        else
        {
            if (textReference.text.Contains(matchText))
                onTextMatch.Invoke();
        }
    }

}
