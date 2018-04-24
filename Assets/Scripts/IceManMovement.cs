using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceManMovement : MonoBehaviour
{
    public MoveLeftHand leftHandScript;
    public MoveRightHand rightHandScript;
    public Transform leftHandPosition, rightHandPosition;
    public float AccTime, AccSpeed, baseSpeed, handSpeed;

    private Vector3 leftHandPivotPostion, rightHandPivotPostion;
    private bool firstPressLeft, firstPressRight, isAcceleratingRight = false, isAcceleratingLeft = false;
    private Vector3 leftHandDirection, rightHandDirection;
    private float currentLeftSpeed, currentRightSpeed, leftHandSpeed, rightHandSpeed;
    private Rigidbody playerRB;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        firstPressLeft = false;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (leftHandScript.IsPressedLeft)
        {
            leftHandSpeed = handSpeed;
            if (!firstPressLeft)
            {
                firstPressLeft = true;
                leftHandPivotPostion = leftHandPosition.localPosition;
            }

            if (currentLeftSpeed < leftHandSpeed && !isAcceleratingLeft)
            {
                isAcceleratingLeft = true;
                StartCoroutine(AccelerateLeft());
            }

            leftHandDirection = (leftHandPosition.localPosition - leftHandPivotPostion);

        }
        else
        {
            leftHandSpeed = 0;
            firstPressLeft = false;
            if(!isAcceleratingLeft)
            {
                isAcceleratingLeft = true;
                StartCoroutine(DeccelerateLeft());
            }
        }

        if (rightHandScript.IsPressedRight)
        {
            rightHandSpeed = handSpeed;
            if (!firstPressRight)
            {
                firstPressRight = true;
                rightHandPivotPostion = rightHandPosition.localPosition;
            }
            if (currentRightSpeed < rightHandSpeed && !isAcceleratingRight)
            {
                isAcceleratingRight = true;
                StartCoroutine(AccelerateRight());
            }
            rightHandDirection = (rightHandPosition.localPosition - rightHandPivotPostion);
        }
        else
        {
            rightHandSpeed = 0;
            firstPressRight = false;
            if(!isAcceleratingRight)
            {
                isAcceleratingRight = true;
                StartCoroutine(DeccelerateRight());
            }
        }

        playerRB.velocity = (baseSpeed * transform.forward) + (currentLeftSpeed * leftHandDirection) + (currentRightSpeed * rightHandDirection);

    }

    IEnumerator AccelerateLeft()
    {
        yield return new WaitForSeconds(AccTime);
        currentLeftSpeed += AccSpeed;
        isAcceleratingLeft = false;
    }

    IEnumerator DeccelerateLeft()
    {
        yield return new WaitForSeconds(AccTime);
        if (currentLeftSpeed > leftHandSpeed)
        {
            currentLeftSpeed -= AccSpeed;
        }
        isAcceleratingLeft = false;
    }

    IEnumerator AccelerateRight()
    {
        yield return new WaitForSeconds(AccTime);
        currentRightSpeed += AccSpeed;
        isAcceleratingRight = false;
    }
    
    IEnumerator DeccelerateRight()
    {
        yield return new WaitForSeconds(AccTime);
        if (currentRightSpeed > rightHandSpeed)
        {
            currentRightSpeed -= AccSpeed;
        }
        isAcceleratingRight = false;
    }
}
