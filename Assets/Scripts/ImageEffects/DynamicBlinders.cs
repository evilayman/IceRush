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

    public float minIntensity, maxIntensity, xChangeMin, xChangeMax, minDistance, maxDistance;
    private float currentIntensity, leftIntensity, RightIntensity, currentX, leftX, rightX;

    public float shiftBy, incrementBy, numRays, rayDistance;
    void Start ()
    {
        tempSettings = PPP.vignette.settings;
        ChangeIntensity(0,0.5f);
    }
	
	void Update ()
    {
        CreateRaysRight(transform.position, Vector3.right);
        CreateRaysLeft(transform.position, -Vector3.right);
        CalculateIntensity();
        print(currentIntensity);
    }

    void CalculateIntensity()
    {
        leftIntensity = (leftDistance - minDistance) / (maxDistance - minDistance) * (maxIntensity - minIntensity) + minIntensity;
        leftX = (leftDistance - minDistance) / (maxDistance - minDistance) * (xChangeMax - xChangeMin) + xChangeMin;

        RightIntensity = ((rightDistance - minDistance) / (maxDistance - minDistance)) * (maxIntensity - minIntensity) + minIntensity;
        rightX = ((rightDistance - minDistance) / (maxDistance - minDistance)) * (xChangeMax - xChangeMin) + xChangeMin;

        currentX = 0.5f + leftX - rightX;
        currentIntensity = leftIntensity + RightIntensity;
        
        ChangeIntensity(currentIntensity, currentX);
    }

    void CreateRaysRight(Vector3 pos, Vector3 dir)
    {
        var xpos = pos.x + shiftBy;
        var zpos = pos.z;
        for (int i = 0; i < numRays; i++)
        {
            DrawRaysRight(new Vector3(xpos, pos.y, zpos), dir);
            zpos += incrementBy;
        }
    }

    void CreateRaysLeft(Vector3 pos, Vector3 dir)
    {
        var xpos = pos.x - shiftBy;
        var zpos = pos.z;
        for (int i = 0; i < numRays; i++)
        {
            DrawRaysLeft(new Vector3(xpos, pos.y, zpos), dir);
            zpos += incrementBy;
        }
    }

    void DrawRaysRight(Vector3 pos, Vector3 dir)
    {
        RaycastHit hit;

        if (Physics.Raycast(pos, dir, out hit, rayDistance))
        {
            rightDistance = hit.distance;
        }
        else
            rightDistance = minDistance;

        Debug.DrawLine(pos, new Vector3(pos.x - rayDistance, pos.y, pos.z), Color.red);
    }

    void DrawRaysLeft(Vector3 pos, Vector3 dir)
    {
        RaycastHit hit;

        if (Physics.Raycast(pos, dir, out hit, rayDistance))
        {
            leftDistance = hit.distance;
        }
        else
            leftDistance = minDistance;

        Debug.DrawLine(pos, new Vector3(pos.x + rayDistance, pos.y, pos.z), Color.red);
    }

    void ChangeIntensity(float intensity, float xpos)
    {
        tempSettings.intensity = intensity;
        tempSettings.center.x = xpos;
        PPP.vignette.settings = tempSettings;
    }
}
