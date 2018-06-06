using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundriesAlphaChange : MonoBehaviour
{

    [SerializeField]
    private Material material;
    double alpha;
    Color temp;
    bool turnAlphaDown;
    void Start()
    {
        material = gameObject.GetComponent<Renderer>().material;
        alpha = 0;
    }


    private void FixedUpdate()
    {
        if (alpha < 1 && !turnAlphaDown)
        {
            alpha += 0.01;
            temp = material.color;
            temp.a += (float)0.01;
            material.color = temp;
        }
        else
        {
            turnAlphaDown = true;
            alpha -= 0.01;
            temp = material.color;
            temp.a -= (float)0.01;
            material.color = temp;
            if (alpha==0)
            {
                turnAlphaDown = false;
            }
        }

    }
}
