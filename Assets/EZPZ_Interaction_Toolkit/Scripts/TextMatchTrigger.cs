using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextMatchTrigger : MonoBehaviour
{   
    public List<string> matchText =  new List<string>();
    public TextMeshProUGUI textReference;
    public bool strictMatch = false;
    public UnityEvent onTextMatch;

    public void CheckMatch()
    {
        bool match = false;

        if(strictMatch)
        {
            match = true;


            if (matchText.Count == 1)
            {
                if (textReference.text.Equals(matchText))
                    match = true;
            }
            else
            {
                foreach (string s in matchText)
                {
                    if (!textReference.text.Contains(s))
                    {
                        match = false;
                    }
                }
            }
                
            if(match)   
                onTextMatch.Invoke();
        }
        else
        {
            foreach (string s in matchText)
            {
                if (textReference.text.Contains(s))
                {
                    match = true;
                }
            }


            if (match)
                onTextMatch.Invoke();

        }
    }

}
