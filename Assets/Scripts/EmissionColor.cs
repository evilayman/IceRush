using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EmissionColor : MonoBehaviour
{
    public Color emissionColor;
    public float intensity = 1f;
    public List<Material> myMats;
    private Color tempColor;
    private float tempIntensity;

    void Update()
    {
        if(emissionColor != tempColor || intensity != tempIntensity)
        {
            tempColor = emissionColor;
            tempIntensity = intensity;
            for (int i = 0; i < myMats.Count; i++)
            {
                myMats[i].SetColor("_EmissionColor", emissionColor * intensity);
            }
        }
    }
}
