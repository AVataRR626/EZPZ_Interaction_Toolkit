//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 14 Nov 2022

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PathTracer : MonoBehaviour
{
    public LineRenderer myLineRenderer;
    public float traceDistanceThreshold = 0.01f;
    public Vector3 lastTracePos;
    public List<Vector3> positions;

    // Start is called before the first frame update
    void Start()
    {
        myLineRenderer = GetComponent<LineRenderer>();
        positions = new List<Vector3>();
        positions.Add(transform.position);
    }

    // Update is called once per frame
    void Update()
    {   
        if(Vector3.Distance(transform.position,lastTracePos) >= traceDistanceThreshold)
        {
            positions.Add(transform.position);
            lastTracePos = transform.position;
            myLineRenderer.positionCount = positions.Count;
            myLineRenderer.SetPositions(positions.ToArray());
        }
        
    }
}
