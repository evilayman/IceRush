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

    public float minIntensity, maxIntensity, xChangeMin, xChangeMax, minDistance, maxDistance, lerpTime;
    private float currentIntensity, leftIntensity, RightIntensity, currentX, leftX, rightX, targetIntensity, targetX;

    public float shiftByX, ShiftByZ, incrementBy, numRays, rayDistance;

    private bool leftHit, rightHit;
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
    }

    void CalculateIntensity()
    {
        leftIntensity = (leftDistance - minDistance) / (maxDistance - minDistance) * (maxIntensity - minIntensity) + minIntensity;
        leftX = (leftDistance - minDistance) / (maxDistance - minDistance) * (xChangeMax - xChangeMin) + xChangeMin;

        RightIntensity = ((rightDistance - minDistance) / (maxDistance - minDistance)) * (maxIntensity - minIntensity) + minIntensity;
        rightX = ((rightDistance - minDistance) / (maxDistance - minDistance)) * (xChangeMax - xChangeMin) + xChangeMin;

        targetX = 0.5f + leftX - rightX;
        targetIntensity = leftIntensity + RightIntensity;

        currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, lerpTime);
        currentX = Mathf.Lerp(currentX, targetX, lerpTime);

        ChangeIntensity(currentIntensity, currentX);
    }

    void CreateRaysRight(Vector3 pos, Vector3 dir)
    {
        var xpos = pos.x + shiftByX;
        var zpos = pos.z + ShiftByZ;
        rightHit = false;
        for (int i = 0; i < numRays; i++)
        {
            DrawRaysRight(new Vector3(xpos, pos.y, zpos), dir);
            zpos += incrementBy;
        }

        if (!rightHit)
        {
            rightDistance = minDistance;
        }
    }

    void CreateRaysLeft(Vector3 pos, Vector3 dir)
    {
        var xpos = pos.x - shiftByX;
        var zpos = pos.z + ShiftByZ;
        leftHit = false;
        for (int i = 0; i < numRays; i++)
        {
            DrawRaysLeft(new Vector3(xpos, pos.y, zpos), dir);
            zpos += incrementBy;
        }

        if(!leftHit)
        {
            leftDistance = minDistance;
        }
    }

    void DrawRaysRight(Vector3 pos, Vector3 dir)
    {
        RaycastHit hit;

        if (Physics.Raycast(pos, dir, out hit, rayDistance))
        {
            rightDistance = hit.distance;
            rightHit = true;
        }

        //Debug.DrawLine(pos, new Vector3(pos.x - rayDistance, pos.y, pos.z), Color.red);
        Debug.DrawRay(pos, dir, Color.red);
    }

    void DrawRaysLeft(Vector3 pos, Vector3 dir)
    {
        RaycastHit hit;

        if (Physics.Raycast(pos, dir, out hit, rayDistance))
        {
            leftDistance = hit.distance;
            leftHit = true;
        }

        //Debug.DrawLine(pos, new Vector3(pos.x + rayDistance, pos.y, pos.z), Color.red);
        Debug.DrawRay(pos, dir, Color.red);

    }

    void ChangeIntensity(float intensity, float xpos)
    {
        tempSettings.intensity = intensity;
        tempSettings.center.x = xpos;
        PPP.vignette.settings = tempSettings;
    }
}
