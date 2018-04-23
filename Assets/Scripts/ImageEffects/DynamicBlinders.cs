using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class DynamicBlinders : MonoBehaviour
{

    public PostProcessingProfile PPP;
    private VignetteModel.Settings tempSettings;

    private float leftDistance;
    private float rightDistance;

    public float LeftDistance
    {
        get
        {
            return leftDistance;
        }

        set
        {
            leftDistance = value;
        }
    }
    public float RightDistance
    {
        get
        {
            return rightDistance;
        }

        set
        {
            rightDistance = value;
        }
    }

    public float minIntensity, maxIntensity, xChangeMin, xChangeMax;
    private float currentIntensity, leftIntensity, RightIntensity, currentX, leftX, rightX;



    void Start ()
    {
        tempSettings = PPP.vignette.settings;
        ChangeIntensity(0,0.5f);
    }
	
	void Update ()
    {
        leftIntensity = LeftDistance * (maxIntensity - minIntensity) + minIntensity;
        leftX = LeftDistance * (xChangeMax - xChangeMin) + xChangeMin;

        RightIntensity = rightDistance * (maxIntensity - minIntensity) + minIntensity;
        rightX = rightDistance * (xChangeMax - xChangeMin) + xChangeMin;

        currentX = 0.5f + leftX - rightX;
        currentIntensity = leftIntensity + RightIntensity;

        ChangeIntensity(currentIntensity, currentX);
    }

    void ChangeIntensity(float intensity, float xpos)
    {
        tempSettings.intensity = intensity;
        tempSettings.center.x = xpos;
        PPP.vignette.settings = tempSettings;
    }
}
