using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Movement : MonoBehaviour
{
    private PlayerManager playerManager;

    private Stats myStats;

    private VRTK_ControllerEvents leftHandController, rightHandController;

    private Transform leftHandTransform, rightHandTransform;

    private ParticleSystem leftHandParticles, rightHandParticles;

    private Rigidbody playerRB;

    private float currentbaseSpeed, currentLeftSpeed, currentRightSpeed;

    private Vector3 leftHandDirection, rightHandDirection;

    private CooldownTimer canAccBoost, canDecBoost, canAccLeft, canAccRight, canDecLeft, canDecRight;

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

        BoostSpeedCheck();
        HandCheck(leftHandController.triggerPressed, ref leftHandDirection, leftHandTransform.forward, leftHandParticles, canAccLeft, canDecLeft, ref currentLeftSpeed);
        HandCheck(rightHandController.triggerPressed, ref rightHandDirection, rightHandTransform.forward, rightHandParticles, canAccRight, canDecRight, ref currentRightSpeed);
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