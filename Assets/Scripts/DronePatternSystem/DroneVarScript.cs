using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct DroneVar
{
    [Space(5)]
    [Header("Movement Stats")]
    public bool move;
    public Vector3 startPoint, endPoint;
    public float Speed;

    [Header("Rotation Stats")]
    [Space(5)]
    public bool rotate;
    public float rotationSpeed;
    [Range(0, 1)]
    public float rAxisX, rAxisY, rAxisZ;

    [Header("Rotation Around Stats")]
    [Space(5)]
    public bool rotateAround;
    public Transform Sun;
    public float rotateSpeed;
    [Range(0, 1)]
    public float raAxisX, raAxisY, raAxisZ;

}

[System.Serializable]
public struct ShapeTransform
{
    public Vector3 position, rotation;
    public ShapeTransform(Vector3 position, Vector3 rotation)
    {
        this.position = position;
        this.rotation = rotation;
    }
}

public class DroneVarScript : MonoBehaviour
{
    private float rAxisXT, rAxisYT, rAxisZT;
    private float raAxisXT, raAxisYT, raAxisZT;
    private Vector3 target, startPointT, endPointT;

    public DroneVar myDronVar = new DroneVar();

    private void Start()
    {
        rAxisXT = myDronVar.rAxisX;
        rAxisYT = myDronVar.rAxisY;
        rAxisZT = myDronVar.rAxisZ;

        raAxisXT = myDronVar.raAxisX;
        raAxisYT = myDronVar.raAxisY;
        raAxisZT = myDronVar.raAxisZ;

        startPointT = myDronVar.startPoint;
        endPointT = myDronVar.endPoint;

        target = myDronVar.endPoint;
    }

    public void Update()
    {
        if (myDronVar.rotate)
        {
            transform.Translate(new Vector3(myDronVar.rAxisX, myDronVar.rAxisY, myDronVar.rAxisZ) * myDronVar.rotationSpeed * Time.deltaTime);
        }

        CheckRotationChange();

        if (myDronVar.rotateAround)
        {
            if (myDronVar.Sun)
                transform.RotateAround(myDronVar.Sun.position, new Vector3(myDronVar.raAxisX, myDronVar.raAxisY, myDronVar.raAxisZ), myDronVar.rotateSpeed * Time.deltaTime);
        }

        if (myDronVar.move)
        {
            Movement();
        }

        CheckMovementChange();
    }

    private void CheckRotationChange()
    {
        if (myDronVar.rAxisX != rAxisXT || myDronVar.rAxisY != rAxisYT || myDronVar.rAxisZ != rAxisZT)
        {
            rAxisXT = myDronVar.rAxisX;
            rAxisYT = myDronVar.rAxisY;
            rAxisZT = myDronVar.rAxisZ;

            transform.rotation = Quaternion.identity;
        }

        if (myDronVar.raAxisX != raAxisXT || myDronVar.raAxisY != raAxisYT || myDronVar.raAxisZ != raAxisZT)
        {
            raAxisXT = myDronVar.raAxisX;
            raAxisYT = myDronVar.raAxisY;
            raAxisZT = myDronVar.raAxisZ;

            transform.rotation = Quaternion.identity;
        }
    }

    private void Movement()
    {
        if (transform.localPosition == myDronVar.endPoint)
        {
            target = myDronVar.startPoint;
        }
        else if (transform.localPosition == myDronVar.startPoint)
        {
            target = myDronVar.endPoint;
        }

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, myDronVar.Speed * Time.deltaTime);
    }

    private void CheckMovementChange()
    {
        if (myDronVar.startPoint != startPointT || myDronVar.endPoint != endPointT)
        {
            startPointT = myDronVar.startPoint;
            endPointT = myDronVar.endPoint;
            target = myDronVar.endPoint;
        }
    }
}
