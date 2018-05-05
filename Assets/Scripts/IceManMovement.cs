using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class IceManMovement : MonoBehaviour
{
    private PlayerManager playerManager;

    private Stats myStats;

    private VRTK_ControllerEvents leftHandController, rightHandController;

    private Transform leftHandTransform, rightHandTransform;

    private ParticleSystem leftHandParticles, rightHandParticles;

    public Transform leftHandPosition, rightHandPosition;
    public float AccTime, AccSpeed, baseSpeed, handSpeed;

    private Vector3 leftHandPivotPostion, rightHandPivotPostion;
    private bool firstPressLeft, firstPressRight, isAcceleratingRight = false, isAcceleratingLeft = false;
    private Vector3 leftHandDirection, rightHandDirection;
    private float currentLeftSpeed, currentRightSpeed, leftHandSpeed, rightHandSpeed;
    private Rigidbody playerRB;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        myStats = playerManager.myStats;

        playerRB = GetComponent<Rigidbody>();

        leftHandController = playerManager.leftHand.GetComponent<VRTK_ControllerEvents>();
        rightHandController = playerManager.rightHand.GetComponent<VRTK_ControllerEvents>();

        leftHandTransform = playerManager.leftHand.GetComponent<Transform>();
        rightHandTransform = playerManager.rightHand.GetComponent<Transform>();

        leftHandParticles = playerManager.leftHand.GetComponentInChildren<ParticleSystem>();
        rightHandParticles = playerManager.rightHand.GetComponentInChildren<ParticleSystem>();

        firstPressLeft = false;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        if (leftHandController.triggerPressed)
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

        if (rightHandController.triggerPressed)
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
