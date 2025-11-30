using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TextMatchTrigger : MonoBehaviour
{   
    public List<string> matchText =  new List<string>();
    public TextMeshProUGUI textReferenceUGUI;
    public TextMeshPro textReference;
    public bool strictMatch = false;
    public UnityEvent onTextMatch;
    public UnityEvent onTextNotMatch;

    public void CheckMatchTMPG()
    {
        bool match = false;

        if (strictMatch)
        {
            match = true;


            if (matchText.Count == 1)
            {
                if (textReferenceUGUI.text.Equals(matchText))
                    match = true;
            }
            else
            {
                foreach (string s in matchText)
                {
                    if (!textReferenceUGUI.text.Contains(s))
                    {
                        match = false;
                    }
                }
            }

            if (match)
                onTextMatch.Invoke();
            else
                onTextNotMatch.Invoke();
        }
        else
        {
            foreach (string s in matchText)
            {
                if (textReferenceUGUI.text.Contains(s))
                {
                    match = true;
                }
            }


            if (match)
                onTextMatch.Invoke();
            else
                onTextNotMatch.Invoke();

        }
    }

    public void CheckMatchTMP()
    {
        bool match = false;

        if (strictMatch)
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

            if (match)
                onTextMatch.Invoke();
            else
                onTextNotMatch.Invoke();
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
            else
                onTextNotMatch.Invoke();

        }
    }

    public void CheckMatch()
    {
        if (textReference != null)
            CheckMatchTMP();

        if (textReferenceUGUI != null)
            CheckMatchTMPG();
    }

}
