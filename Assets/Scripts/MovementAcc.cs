using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MovementAcc : MonoBehaviour
{
    private PlayerManager playerManager;

    private Stats myStats;

    private VRTK_ControllerEvents leftHandController, rightHandController;

    private Transform leftHandTransform, rightHandTransform;

    private ParticleSystem leftHandParticles, rightHandParticles;

    private Rigidbody playerRB;

    private float currentbaseSpeed, currentLeftSpeed, currentRightSpeed;

    private Vector3 leftHandDirection, rightHandDirection;

    private CooldownTimer canDec, canAccBoost, canDecBoost, canAccLeft, canDecLeft, canAccRight, canDecRight;

    private bool gameStarted;

    void Start()
    {
        init();
    }

    void init()
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
        if (leftHandController.AnyButtonPressed() || rightHandController.AnyButtonPressed())
            gameStarted = true;

        BoostRate(ref currentbaseSpeed, (playerManager.InBoostRegion) ? myStats.boostSpeed : 0, canAccBoost, canDecBoost, myStats.accRateBoost, myStats.decRateBoost);

        HandRate(leftHandController.triggerPressed, ref leftHandDirection, leftHandTransform.forward, leftHandParticles, canAccLeft, canDecLeft, ref currentLeftSpeed);
        HandRate(rightHandController.triggerPressed, ref rightHandDirection, rightHandTransform.forward, rightHandParticles, canAccRight, canDecRight, ref currentRightSpeed);
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

    void BoostRate(ref float current, float target, CooldownTimer canAcc, CooldownTimer canDec, float accRate, float decRate)
    {
        if (current < target && canAcc.IsReady())
        {
            canAcc.Reset();
            current += accRate;
        }
        else if (current > target && canDec.IsReady())
        {
            canDec.Reset();
            current -= decRate;
        }
        if (Mathf.Abs(current) < decRate)
        {
            current = 0;
        }
        print(current);
    }

    void HandRate(bool triggerPressed, ref Vector3 handDirection, Vector3 handTransform, ParticleSystem emission, CooldownTimer canAccHand, CooldownTimer canDecHand, ref float currentHandSpeed)
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
        }
    }
}