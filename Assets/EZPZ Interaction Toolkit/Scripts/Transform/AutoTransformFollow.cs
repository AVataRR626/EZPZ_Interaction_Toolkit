//EZPZ Interaction Toolkit
//by Matt Cabanag
//created 27 May 2025

using UnityEngine;

[RequireComponent (typeof(TransformFollow))]
public class AutoTransformFollow : MonoBehaviour
{
    public string autoFindTag = "Player";
    public GameObject subject;
    public TransformFollow myFollower;

    private void Start()
    {
        if(myFollower == null)
            myFollower = GetComponent<TransformFollow>();

        subject = GameObject.FindGameObjectWithTag(autoFindTag);

        myFollower.subject = subject.transform;
    }
}
