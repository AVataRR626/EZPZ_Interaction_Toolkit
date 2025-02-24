using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenLineGun : MonoBehaviour
{
    public PathTracer currentLine;
    public PathTracer pathTracerPrefab;
    public Transform penTip;


    // Update is called once per frame
    void Update()
    {
        if (currentLine != null)
            currentLine.transform.position = penTip.position;
    }

    [ContextMenu("StartLine")]
    public void StartLine()
    {
        currentLine = Instantiate(pathTracerPrefab, penTip.position, penTip.rotation);
    }

    [ContextMenu("EndLine")]
    public void EndLine()
    {
        currentLine = null;
    }
}
