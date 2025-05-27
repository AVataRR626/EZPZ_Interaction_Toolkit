//Immersive Data (ImDat) Project
//by Matt Cabanag
//Created: 03 March 2025

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteInEditMode]
public class LineRendererLink : MonoBehaviour
{
    public LineRenderer lr;
    public Transform linkPoint;

	// Use this for initialization
	void Start ()
    {
        lr = GetComponent<LineRenderer>();
        lr.useWorldSpace = true;
	}
	
	//
    //Update is called once per frame
	void Update ()
    {
		if(linkPoint != null)
        {
            Vector3[] a = { transform.position, linkPoint.position };
            //draw a line to the end point            
            lr.SetPositions(a);//correct for scale
        }
	}
}
