using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using VRTK.Controllables.ArtificialBased;

public class MovementLevers : MonoBehaviour
{
    public VRTK_ArtificialRotator xLever, yLever, zLever;

    private PlayerManager playerManager;

    private Stats myStats;

    private VRTK_ControllerEvents leftHandController, rightHandController;

    private Transform leftHandTransform, rightHandTransform;

    private ParticleSystem leftHandParticles, rightHandParticles;

    private Rigidbody playerRB;

    private float currentbaseSpeed, currentXSpeed, currentYSpeed, currentZSpeed;

    private CooldownTimer canDec, canAccBoost, canDecBoost, canAccX, canDecX, canAccY, canDecY, canAccZ, canDecZ;

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

        canAccX = canAccY = canAccZ = new CooldownTimer(myStats.accTimeLever);
        canDecX = canDecY = canDecZ = new CooldownTimer(myStats.decTimeLever);
    }

    void Update()
    {
        if (leftHandController.AnyButtonPressed() || rightHandController.AnyButtonPressed())
            gameStarted = true;

        AccDecRate(ref currentbaseSpeed, myStats.baseSpeed, canAccBoost, canDecBoost, myStats.accRateBoost, myStats.decRateBoost);
        AccDecRate(ref currentXSpeed, myStats.leverStepSpeed * xLever.GetStepValue(xLever.GetValue()), canAccX, canDecX, myStats.accRateLever, myStats.decRateLever);
        AccDecRate(ref currentYSpeed, myStats.leverStepSpeed * yLever.GetStepValue(yLever.GetValue()), canAccY, canDecY, myStats.accRateLever, myStats.decRateLever);
        AccDecRate(ref currentZSpeed, myStats.leverStepSpeed * zLever.GetStepValue(zLever.GetValue()), canAccZ, canDecZ, myStats.accRateLever, myStats.decRateLever);
    }

    private void FixedUpdate()
    {
        if (gameStarted)
        {
            playerRB.velocity += (transform.forward * (currentbaseSpeed + currentZSpeed)) + (transform.up * currentYSpeed);
            playerRB.MoveRotation(playerRB.rotation * Quaternion.Euler(Vector3.up * currentXSpeed * 75 * Time.deltaTime));

            Decelerate();
            LimitSpeed(myStats.maxSpeed);
        }
    }

    void AccDecRate(ref float current, float target, CooldownTimer canAcc, CooldownTimer canDec, float accRate, float decRate)
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
}