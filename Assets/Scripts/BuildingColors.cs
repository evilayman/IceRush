using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BuildingColors : MonoBehaviour
{
    public Color color, emissionColor;
    public float intensity = 1f;
    public List<Material> myMats;
    private Color tempEmissionColor, tempColor;
    private float tempIntensity;

    void Update()
    {
        if(emissionColor != tempEmissionColor || intensity != tempIntensity || color != tempColor)
        {
            tempColor = color;
            tempEmissionColor = emissionColor;
            tempIntensity = intensity;
            for (int i = 0; i < myMats.Count; i++)
            {
                myMats[i].SetColor("_Color", color);
                myMats[i].SetColor("_EmissionColor", emissionColor * intensity);
            }
        }
    }
}
