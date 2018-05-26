﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MovementForNetwork : Photon.MonoBehaviour
{
    private GameManager GM;

    private PlayerManagerForNetwork playerManager;

    private Stats myStats;

    private VRTK_ControllerEvents leftHandController, rightHandController;

    private Transform leftHandTransform, rightHandTransform;

    private ParticleSystem leftHandParticles, rightHandParticles;

    private Rigidbody playerRB;

    private float currentbaseSpeed, currentLeftSpeed, currentRightSpeed, handSpeed, lag;

    private Vector3 leftHandDirection, rightHandDirection, targetPos;

    private CooldownTimer canDec, canAccBoost, canDecBoost, canAccLeft, canDecLeft, canAccRight, canDecRight;

    private bool pressedRight, pressedLeft;

    private PlayerManagerForNetwork.PlayerState tempPlayerState;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (photonView.isMine || GM.Offline)
            init();
    }

    void init()
    {
        playerManager = GetComponent<PlayerManagerForNetwork>();

        playerRB.isKinematic = true;

        myStats = playerManager.myStats;
        handSpeed = myStats.handSpeed;

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
        if (photonView.isMine || GM.Offline)
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
                pressedRight = true;
            else if (Input.GetKeyUp(KeyCode.Mouse1))
                pressedRight = false;

            if (Input.GetKeyDown(KeyCode.Mouse0))
                pressedLeft = true;
            else if (Input.GetKeyUp(KeyCode.Mouse0))
                pressedLeft = false;

            CheckStateChange();

            HandRate(pressedLeft, ref leftHandDirection, leftHandTransform.forward, leftHandParticles, canAccLeft, canDecLeft, ref currentLeftSpeed);
            HandRate(pressedRight, ref rightHandDirection, rightHandTransform.forward, rightHandParticles, canAccRight, canDecRight, ref currentRightSpeed);
            AccDecRate(ref currentbaseSpeed, (playerManager.InBoostRegion) ? myStats.boostSpeed : 0, canAccBoost, canDecBoost, myStats.accRateBoost, myStats.decRateBoost);

            //HandRate(leftHandController.triggerPressed, ref leftHandDirection, leftHandTransform.forward, leftHandParticles, canAccLeft, canDecLeft, ref currentLeftSpeed);
            //HandRate(rightHandController.triggerPressed, ref rightHandDirection, rightHandTransform.forward, rightHandParticles, canAccRight, canDecRight, ref currentRightSpeed);
        }
    }

    void CheckStateChange()
    {
        if (tempPlayerState != playerManager.currentPlayerState)
        {
            tempPlayerState = playerManager.currentPlayerState;

            switch (playerManager.currentPlayerState)
            {
                case PlayerManagerForNetwork.PlayerState.Stopped:
                    playerRB.isKinematic = true;
                    break;
                case PlayerManagerForNetwork.PlayerState.Normal:
                    playerRB.isKinematic = false;
                    handSpeed = myStats.handSpeed;
                    break;
                case PlayerManagerForNetwork.PlayerState.Slowed:
                    playerRB.isKinematic = false;
                    handSpeed = ((100 - myStats.slowPercent) * myStats.handSpeed) / 100;
                    break;
                default:
                    break;
            }
        }
    }

    private void FixedUpdate()
    {
        if (photonView.isMine || GM.Offline)
        {
            playerRB.velocity += (transform.forward * currentbaseSpeed) + (leftHandDirection * currentLeftSpeed) + (rightHandDirection * currentRightSpeed);
            Decelerate();
            LimitSpeed(myStats.maxSpeed);
        }
        else
            SmoothNetMovement();
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

    void HandRate(bool triggerPressed, ref Vector3 handDirection, Vector3 handTransform, ParticleSystem emission, CooldownTimer canAccHand, CooldownTimer canDecHand, ref float currentHandSpeed)
    {
        if (triggerPressed)
        {
            if (!emission.isPlaying)
                emission.Play();

            handDirection = handTransform;

            if (canAccHand.IsReady() && currentHandSpeed < handSpeed)
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

    private void SmoothNetMovement()
    {
        if (Vector3.Distance(playerRB.position, targetPos) > 10)
        {
            playerRB.position = targetPos;
        }
        else
        {
            playerRB.position = Vector3.MoveTowards(playerRB.position, targetPos, Time.fixedDeltaTime);
        }
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (playerRB)
        {
            if (stream.isWriting)
            {
                stream.SendNext(playerRB.position);
                stream.SendNext(playerRB.velocity);
            }
            else
            {
                targetPos = (Vector3)stream.ReceiveNext();
                playerRB.velocity = (Vector3)stream.ReceiveNext();

                lag = Mathf.Abs((float)(PhotonNetwork.time - info.timestamp));
                targetPos += (playerRB.velocity * lag);
            }
        }
    }
}