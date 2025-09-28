//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 14 Nov 2022


using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GenUtils : MonoBehaviour
{

    public static void LoadSceneIndex(int index)
    {
        SceneManager.LoadScene(index);
    }

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public static string HMSFormat(float seconds, string format)
    {
        string result = "";

        int minTotal = (int)(seconds / 60);
        int hourTotal = (int)(seconds / 3600);
        float secTotal = seconds - (minTotal * 60);

        string minString = minTotal.ToString();
        string hourString = hourTotal.ToString();
        string secString = secTotal.ToString(format);

        Debug.Log("secTotal: " + secTotal);

        if (hourTotal > 0)
            result += hourString + ":";

        if (minTotal > 0)
        {
            if (minTotal < 60)
            {
                if (minTotal < 10)
                {
                    result += "0";
                }

                result += minString + ":";
            }
            else
            {
                result += "00:";
            }
        }

        if (secTotal > 0)
        {
            if(secTotal < 10)
            {
                result += "0";
            }

            result += secString;
        }

        return result;
    }

    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
}
