using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Stats myStats;

    private Rigidbody playerRB;

    private float currentbaseSpeed, currentLeftSpeed, currentRightSpeed;

    private Vector3 leftHandDirection, rightHandDirection;

    private CooldownTimer canAccBoost, canDecBoost, canAccLeft, canAccRight, canDecLeft, canDecRight;

    private bool gameStarted;

    public ParticleSystem emissionLeft, emissionRight;

    void Start()
    {
        init();
    }

    void init()
    {
        myStats = GetComponent<Stats>();
        playerRB = GetComponent<Rigidbody>();

        canAccBoost = new CooldownTimer(myStats.accTimeBoost);
        canDecBoost = new CooldownTimer(myStats.decTimeBoost);

        canAccLeft = new CooldownTimer(myStats.accTimeHand);
        canDecLeft = new CooldownTimer(myStats.decTimeHand);

        canAccRight = new CooldownTimer(myStats.accTimeHand);
        canDecRight = new CooldownTimer(myStats.decTimeHand);
    }

    void Update()
    {
        if (myStats.leftHand.AnyButtonPressed() || myStats.rightHand.AnyButtonPressed())
            gameStarted = true;

        BoostSpeedCheck();
        HandCheck(myStats.leftHand.triggerPressed, ref leftHandDirection, myStats.leftHandTransform.forward, emissionLeft, canAccLeft, canDecLeft, ref currentLeftSpeed);
        HandCheck(myStats.rightHand.triggerPressed, ref rightHandDirection, myStats.rightHandTransform.forward, emissionRight, canAccRight, canDecRight, ref currentRightSpeed);
    }

    private void FixedUpdate()
    {
        if (gameStarted)
            playerRB.velocity = ((transform.forward * currentbaseSpeed) +
                (leftHandDirection * currentLeftSpeed) +
                (rightHandDirection * currentRightSpeed));
    }

    void BoostSpeedCheck()
    {
        if (currentbaseSpeed < myStats.baseSpeed && canAccBoost.IsReady())
        {
            canAccBoost.Reset();
            currentbaseSpeed += myStats.accRateBoost;
        }
        else if (currentbaseSpeed > myStats.baseSpeed && canDecBoost.IsReady())
        {
            canDecBoost.Reset();
            currentbaseSpeed -= myStats.decRateBoost;
        }
    }

    void HandCheck(bool triggerPressed, ref Vector3 handDirection, Vector3 handTransform, ParticleSystem emission, CooldownTimer canAccHand, CooldownTimer canDecHand, ref float currentHandSpeed)
    {
        if (triggerPressed)
        {
            if (!emission.isPlaying)
                emission.Play();

            handDirection = handTransform;

            if (canAccHand.IsReady() && currentHandSpeed < myStats.handSpeed)
            {
                canAccHand.Reset();
                currentHandSpeed += myStats.accRateHand;
            }
        }
        else
        {
            if (emission.isPlaying)
                emission.Stop();

            if (canDecHand.IsReady() && currentHandSpeed > 0)
            {
                canDecHand.Reset();
                currentHandSpeed -= myStats.decRateHand;
            }
        }
    }

}