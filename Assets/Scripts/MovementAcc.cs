using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementAcc : MonoBehaviour
{
    private Stats myStats;

    private Rigidbody playerRB;

    private float currentbaseSpeed, currentLeftSpeed, currentRightSpeed;

    private Vector3 leftHandDirection, rightHandDirection;

    private CooldownTimer canDec, canAccBoost, canDecBoost, canAccLeft, canDecLeft, canAccRight, canDecRight;

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

        canDec = new CooldownTimer(myStats.decTime);

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

        print(playerRB.velocity);
        print(currentbaseSpeed);

        BoostSpeedCheck();
        HandCheck(myStats.leftHand.triggerPressed, ref leftHandDirection, myStats.leftHandTransform.forward, emissionLeft, canAccLeft, canDecLeft, ref currentLeftSpeed);
        HandCheck(myStats.rightHand.triggerPressed, ref rightHandDirection, myStats.rightHandTransform.forward, emissionRight, canAccRight, canDecRight, ref currentRightSpeed);
    }

    private void FixedUpdate()
    {
        if (gameStarted)
        {
            playerRB.velocity += (transform.forward * currentbaseSpeed) + (leftHandDirection * currentLeftSpeed) + (rightHandDirection * currentRightSpeed);

            Decelerate();
            LimitSpeed(myStats.maxSpeed);
        }
    }

    void Decelerate()
    {
        if (canDec.IsReady())
        {
            canDec.Reset();

            Vector3 tempV = playerRB.velocity;

            if (tempV.x > 0 && tempV.x != 0 && !(Mathf.Abs(tempV.x) < myStats.decRate))
                tempV.x -= myStats.decRate;
            else if (tempV.x < 0 && tempV.x != 0 && !(Mathf.Abs(tempV.x) < myStats.decRate))
                tempV.x += myStats.decRate;
            else
                tempV.x = 0;

            if (tempV.y > 0 && tempV.y != 0 && !(Mathf.Abs(tempV.y) < myStats.decRate))
                tempV.y -= myStats.decRate;
            else if (tempV.y < 0 && tempV.y != 0 && !(Mathf.Abs(tempV.y) < myStats.decRate))
                tempV.y += myStats.decRate;
            else
                tempV.y = 0;

            if (tempV.z > 0 && tempV.z != 0 && !(Mathf.Abs(tempV.z) < myStats.decRate))
                tempV.z -= myStats.decRate;
            else if (tempV.z < 0 && tempV.z != 0 && !(Mathf.Abs(tempV.z) < myStats.decRate))
                tempV.z += myStats.decRate;
            else
                tempV.z = 0;

            playerRB.velocity = tempV;
        }
    }

    void LimitSpeed(float maxSpeed)
    {
        Vector3 tempV = playerRB.velocity;
        if (tempV.x > maxSpeed)
            tempV.x = maxSpeed;
        else if (tempV.x < -maxSpeed)
            tempV.x = -maxSpeed;
        if (tempV.y > maxSpeed)
            tempV.y = maxSpeed;
        else if (tempV.y < -maxSpeed)
            tempV.y = -maxSpeed;
        if (tempV.z > maxSpeed)
            tempV.z = maxSpeed;
        else if (tempV.z < -maxSpeed)
            tempV.z = -maxSpeed;
        playerRB.velocity = tempV;
    }

    void BoostSpeedCheck()
    {
        if (currentbaseSpeed < myStats.baseSpeed && canAccBoost.IsReady())
        {
            canAccBoost.Reset();
            currentbaseSpeed += myStats.accRateBoost;
        }
        else if (currentbaseSpeed > myStats.baseSpeed && canDecBoost.IsReady() && !(Mathf.Abs(currentbaseSpeed) < myStats.decRateBoost))
        {
            canDecBoost.Reset();
            currentbaseSpeed -= myStats.decRateBoost;
        }
        else if(Mathf.Abs(currentbaseSpeed) < myStats.decRateBoost)
        {
            currentbaseSpeed = 0;
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

            currentHandSpeed = 0;

            //if (canDecHand.IsReady() && currentHandSpeed > 0)
            //{
            //    canDecHand.Reset();
            //    currentHandSpeed -= myStats.decRateHand;
            //}
        }
    }
}