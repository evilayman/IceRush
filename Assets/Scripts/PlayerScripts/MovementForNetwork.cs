using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class MovementForNetwork : Photon.MonoBehaviour
{
    private GameManager GM;

    private PlayerManagerForNetwork playerManager;

    private Stats myStats;

    private VRTK_ControllerEvents leftHandController, rightHandController;

    private Transform leftHandTransform, rightHandTransform, headTransform;

    private ParticleSystem leftHandParticles, rightHandParticles;

    private Rigidbody playerRB;

    private float currentbaseSpeed, currentLeftSpeed, currentRightSpeed, handSpeed, lag, lerpSmooth = 0.5f;

    private Vector3 leftHandDirection, rightHandDirection, targetPos,
        targetHeadPos, targetLHPos, targetLHRot, targetRHPos, targetRHRot;

    private CooldownTimer canDec, canAccBoost, canDecBoost, canAccLeft, canDecLeft, canAccRight, canDecRight;

    private bool pressedRight, pressedLeft, mousePressedLeft, mousePressedRight;

    private PlayerManagerForNetwork.PlayerState tempPlayerState;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        playerManager = GetComponent<PlayerManagerForNetwork>();

        headTransform = playerManager.headTarget.GetComponent<Transform>();
        leftHandTransform = playerManager.leftHand.GetComponent<Transform>();
        rightHandTransform = playerManager.rightHand.GetComponent<Transform>();

        if (photonView.isMine || GM.Offline)
            init();
    }

    void init()
    {

        playerRB.isKinematic = true;

        myStats = playerManager.myStats;
        handSpeed = myStats.handSpeed;

        leftHandController = playerManager.leftHand.GetComponent<VRTK_ControllerEvents>();
        rightHandController = playerManager.rightHand.GetComponent<VRTK_ControllerEvents>();

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
        //Debug.Log("Velocity" + playerRB.velocity + "\tBoostSpeed: " + currentbaseSpeed);
        if (photonView.isMine || GM.Offline)
        {
            mousePressedLeft = Input.GetKey(KeyCode.Mouse0) ? true : false;
            mousePressedRight = Input.GetKey(KeyCode.Mouse1) ? true : false;

            pressedLeft = (mousePressedLeft || leftHandController.triggerPressed) ? true : false;
            pressedRight = (mousePressedRight || rightHandController.triggerPressed) ? true : false;

            CheckStateChange();

            HandRate(pressedLeft, ref leftHandDirection, leftHandTransform.forward, leftHandParticles, canAccLeft, canDecLeft, ref currentLeftSpeed);
            HandRate(pressedRight, ref rightHandDirection, rightHandTransform.forward, rightHandParticles, canAccRight, canDecRight, ref currentRightSpeed);
            AccDecRate(ref currentbaseSpeed, (playerManager.InBoostRegion) ? myStats.boostSpeed : 0, canAccBoost, canDecBoost, myStats.accRateBoost, myStats.decRateBoost);
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
                case PlayerManagerForNetwork.PlayerState.SlowToStop:
                    playerRB.isKinematic = false;
                    handSpeed = 0;
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
        {
            SmoothNetMovement();
            SyncHeadHand();
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
            else if (canDecHand.IsReady() && currentHandSpeed > handSpeed)
            {
                canDecHand.Reset();
                currentHandSpeed -= myStats.decRateHand;
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

    private void SyncHeadHand()
    {
        headTransform.position = Vector3.Lerp(headTransform.position, targetHeadPos, lerpSmooth);

        leftHandTransform.position = Vector3.Lerp(leftHandTransform.position, targetLHPos, lerpSmooth);
        leftHandTransform.rotation = Quaternion.Euler(Vector3.Lerp(leftHandTransform.rotation.eulerAngles, targetLHRot, lerpSmooth));

        rightHandTransform.position = Vector3.Lerp(rightHandTransform.position, targetRHPos, lerpSmooth);
        rightHandTransform.rotation = Quaternion.Euler(Vector3.Lerp(rightHandTransform.rotation.eulerAngles, targetRHRot, lerpSmooth));
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (playerRB)
        {
            if (stream.isWriting)
            {
                stream.SendNext(playerRB.position);
                stream.SendNext(playerRB.velocity);

                stream.SendNext(headTransform.position);

                stream.SendNext(leftHandTransform.position);
                stream.SendNext(leftHandTransform.rotation.eulerAngles);

                stream.SendNext(rightHandTransform.position);
                stream.SendNext(rightHandTransform.rotation.eulerAngles);
            }
            else
            {
                targetPos = (Vector3)stream.ReceiveNext();
                playerRB.velocity = (Vector3)stream.ReceiveNext();

                targetHeadPos = (Vector3)stream.ReceiveNext();

                targetLHPos = (Vector3)stream.ReceiveNext();
                targetLHRot = (Vector3)stream.ReceiveNext();

                targetRHPos = (Vector3)stream.ReceiveNext();
                targetRHRot = (Vector3)stream.ReceiveNext();

                lag = Mathf.Abs((float)(PhotonNetwork.time - info.timestamp));
                targetPos += (playerRB.velocity * lag);
            }
        }
    }
}