using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class XRSprintLocomotion : MonoBehaviour
{
    public DynamicMoveProvider myDMP;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(myDMP == null)
        {
            myDMP = GetComponent<DynamicMoveProvider>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
