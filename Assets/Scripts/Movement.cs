using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Stats myStats;

    private Rigidbody playerRB;

    private float CurrentbaseSpeed, CurrentLeftSpeed, CurrentRightSpeed;

    private Vector3 LeftHandDirection, RightHandDirection;

    private bool isAccelerating = false, isAcceleratingLeft = false, isAcceleratingRight = false;
    public bool GameStarted = true;

    public ParticleSystem emissionLeft, emissionRight;

    void Start()
    {
        myStats = GetComponent<Stats>();
        playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        BaseSpeedCheck();
        LeftHandCheck();
        RightHandCheck();
    }

    private void FixedUpdate()
    {
        if (GameStarted)
            playerRB.velocity = ((transform.forward * CurrentbaseSpeed) + (LeftHandDirection * CurrentLeftSpeed) + (RightHandDirection * CurrentRightSpeed));
    }

    void BaseSpeedCheck()
    {
        if (!isAccelerating)
        {
            isAccelerating = true;

            if (CurrentbaseSpeed < myStats.baseSpeed)
            {
                CurrentbaseSpeed += myStats.accRate;
            }
            else if (CurrentbaseSpeed > myStats.baseSpeed)
            {
                CurrentbaseSpeed -= myStats.decRate;
            }

            StartCoroutine(Accelerate(myStats.accTime));
        }
    }
    void LeftHandCheck()
    {
        if (myStats.leftHandScript.IsTriggerPressed)
        {
            if (!emissionLeft.isPlaying)
                emissionLeft.Play();

            LeftHandDirection = myStats.leftHandTrigger.forward;

            if (!isAcceleratingLeft && CurrentLeftSpeed < myStats.handSpeed)
            {
                isAcceleratingLeft = true;

                CurrentLeftSpeed += myStats.accRateHand;

                StartCoroutine(AccelerateLeft(myStats.accTimeHand));
            }

            if (!GameStarted)
                GameStarted = true;
        }
        else
        {
            if (emissionLeft.isPlaying)
                emissionLeft.Stop();

            if (!isAcceleratingLeft && CurrentLeftSpeed > 0)
            {
                isAcceleratingLeft = true;

                CurrentLeftSpeed -= myStats.decRateHand;

                StartCoroutine(AccelerateLeft(myStats.accTimeHand));
            }
        }
    }
    void RightHandCheck()
    {
        if (myStats.rightHandScript.IsTriggerPressed)
        {
            if (!emissionRight.isPlaying)
                emissionRight.Play();

            RightHandDirection = myStats.rightHandTrigger.forward;

            if (!isAcceleratingRight && CurrentRightSpeed < myStats.handSpeed)
            {
                isAcceleratingRight = true;

                CurrentRightSpeed += myStats.accRateHand;

                StartCoroutine(AccelerateRight(myStats.accTimeHand));
            }

            if (!GameStarted)
                GameStarted = true;
        }
        else
        {
            if (emissionRight.isPlaying)
                emissionRight.Stop();

            if (!isAcceleratingRight && CurrentRightSpeed > 0)
            {
                isAcceleratingRight = true;

                CurrentRightSpeed -= myStats.decRateHand;

                StartCoroutine(AccelerateRight(myStats.accTimeHand));
            }
        }
    }

    IEnumerator Accelerate(float time)
    {
        yield return new WaitForSeconds(time);
        isAccelerating = false;
    }
    IEnumerator AccelerateLeft(float time)
    {
        yield return new WaitForSeconds(time);
        isAcceleratingLeft = false;
    }
    IEnumerator AccelerateRight(float time)
    {
        yield return new WaitForSeconds(time);
        isAcceleratingRight = false;
    }




}