using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class EmissionColor : MonoBehaviour
{
    public Color emissionColor;
    public List<Material> myMats;
    private Color tempColor;

    void Update()
    {
        if(emissionColor != tempColor)
        {
            tempColor = emissionColor;
            for (int i = 0; i < myMats.Count; i++)
            {
                myMats[i].SetColor("_EmissionColor", emissionColor);
            }
        }
    }
}
