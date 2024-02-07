using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmmsiveMaterialModifier : MonoBehaviour
{
    public Material myMaterial;
    public Color currentEmissionColour;
    public Color[] alternativeColours;

    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<MeshRenderer>().material;

        currentEmissionColour = myMaterial.GetColor("_EmissionColor");
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.SetColor("_EmissionColor", currentEmissionColour);
    }

    public void SetAlternativeColour(int colourIndex)
    {
        currentEmissionColour = alternativeColours[colourIndex];
    }
}
