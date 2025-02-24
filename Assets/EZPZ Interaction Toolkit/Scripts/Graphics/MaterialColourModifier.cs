using UnityEngine;

public class MaterialColourModifier : MonoBehaviour
{
    public Material myMaterial;
    public Color currentMaterialColour;
    public Color[] alternativeColours;

    // Start is called before the first frame update
    void Start()
    {
        myMaterial = GetComponent<Renderer>().material;
        currentMaterialColour = myMaterial.color;
    }

    // Update is called once per frame
    void Update()
    {
        myMaterial.color = currentMaterialColour;
    }

    public void SetAlternativeColour(int colourIndex)
    {
        currentMaterialColour = alternativeColours[colourIndex];
    }
}
