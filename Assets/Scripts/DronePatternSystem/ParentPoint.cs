using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//[ExecuteInEditMode]
public class ParentPoint : MonoBehaviour
{
    [Space(5)]
    private Vector3 startPosition;
    private Vector3 startRotation;
    [Header("Movement Stats")]
    public bool move;
    public Vector3 startPoint, endPoint;
    public float Speed;

    [Header("Rotation Stats")]
    public bool rotate;
    public float rotateSpeed;
    [Range(0,1)]
    public float axisX, axisY, axisZ;
    

    private float axisXT, axisYT, axisZT;
    private Vector3 target, startPointT, endPointT;

    public Vector3 StartPosition
    {
        get
        {
            return startPosition;
        }

        set
        {
            startPosition = value;
        }
    }
    public Vector3 StartRotation
    {
        get
        {
            return startRotation;
        }

        set
        {
            startRotation = value;
        }
    }

    private void Start()
    {
        axisXT = axisX;
        axisYT = axisY;
        axisZT = axisZ;

        startPointT = startPoint;
        endPointT = endPoint;

        target = endPoint;
    }

    public void Update()
    {
        if (rotate)
        {
            transform.Rotate(new Vector3(axisX, axisY, axisZ) * rotateSpeed * Time.deltaTime);
        }


        CheckRotationChange();
        

        if (move)
        {
            Movement();
        }

        CheckMovementChange();

    }

    private void Movement()
    {
        if(transform.localPosition == endPoint)
        {
            target = startPoint;
        }
        else if(transform.localPosition == startPoint)
        {
            target = endPoint;
        }

        transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, Speed * Time.deltaTime);
    }

    private void CheckRotationChange()
    {
        if (axisX != axisXT || axisY != axisYT || axisZ != axisZT)
        {
            axisXT = axisX;
            axisYT = axisY;
            axisZT = axisZ;

            transform.eulerAngles = StartRotation;
        }
    }

    private void CheckMovementChange()
    {
        if (startPoint != startPointT || endPoint != endPointT)
        {
            startPointT = startPoint;
            endPointT = endPoint;

            target = endPoint;
        }
    }
}
