﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public HandScript leftHandScript, rightHandScript;
    private Rigidbody playerRB;

    public Transform LeftHandTrigger, RightHandTrigger, Player;

    public float HandPushSpeed, baseSpeed, AccSpeed, DecSpeed, AccTime;
    private float CurrentbaseSpeed, CurrentLeftSpeed, CurrentRightSpeed, LeftHandSpeed, RightHandSpeed;

    private Vector3 LeftHandDirection, RightHandDirection;

    private bool isAccelerating = false, isAcceleratingLeft = false, isAcceleratingRight = false,
        GameStarted = true, Died = false;

    public ParticleSystem emissionLeft, emissionRight;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        FadeFromBlack(0.2f);

    }

    void Update()
    {
        if (leftHandScript.IsTriggerPressed)
        {
            if (!emissionLeft.isPlaying)
                emissionLeft.Play();

            LeftHandDirection = LeftHandTrigger.forward;
            LeftHandSpeed = HandPushSpeed;

            if (!isAcceleratingLeft && CurrentLeftSpeed < LeftHandSpeed)
            {
                isAcceleratingLeft = true;
                StartCoroutine(AccelerateLeft());
            }

            if (!GameStarted)
                GameStarted = true;
        }
        else
        {
            if (emissionLeft.isPlaying)
                emissionLeft.Stop();

            LeftHandSpeed = 0;
            if (!isAcceleratingLeft && CurrentLeftSpeed > LeftHandSpeed)
            {
                isAcceleratingLeft = true;
                StartCoroutine(AccelerateLeft());
            }
        }

        if (rightHandScript.IsTriggerPressed)
        {
            if (!emissionRight.isPlaying)
                emissionRight.Play();

            RightHandDirection = RightHandTrigger.forward;
            RightHandSpeed = HandPushSpeed;

            if (!isAcceleratingRight && CurrentRightSpeed < RightHandSpeed)
            {
                isAcceleratingRight = true;
                StartCoroutine(AccelerateRight());
            }

            if (!GameStarted)
                GameStarted = true;
        }
        else
        {
            if (emissionRight.isPlaying)
                emissionRight.Stop();

            RightHandSpeed = 0;
            if (!isAcceleratingRight && CurrentRightSpeed > RightHandSpeed)
            {
                isAcceleratingRight = true;
                StartCoroutine(AccelerateRight());
            }
        }

        if (!isAccelerating)
        {
            isAccelerating = true;
            StartCoroutine(Accelerate());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            baseSpeed += 100;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            baseSpeed -= 100;
        }
    }

    IEnumerator Accelerate()
    {
        yield return new WaitForSeconds(AccTime);

        if (CurrentbaseSpeed < baseSpeed)
        {
            CurrentbaseSpeed += AccSpeed;
        }
        else if (CurrentbaseSpeed > baseSpeed)
        {
            CurrentbaseSpeed -= DecSpeed;
        }
        isAccelerating = false;
    }

    IEnumerator AccelerateLeft()
    {
        yield return new WaitForSeconds(AccTime);

        if (CurrentLeftSpeed < LeftHandSpeed)
        {
            CurrentLeftSpeed += AccSpeed;
        }
        else if (CurrentLeftSpeed > LeftHandSpeed)
        {
            CurrentLeftSpeed -= AccSpeed;
        }
        isAcceleratingLeft = false;
    }

    IEnumerator AccelerateRight()
    {
        yield return new WaitForSeconds(AccTime);

        if (CurrentRightSpeed < RightHandSpeed)
        {
            CurrentRightSpeed += AccSpeed;
        }
        else if (CurrentRightSpeed > RightHandSpeed)
        {
            CurrentRightSpeed -= AccSpeed;
        }
        isAcceleratingRight = false;
    }

    private void FadeToBlack(float time)
    {
        //set start color
        SteamVR_Fade.Start(Color.clear, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.black, time);
    }
    private void FadeFromBlack(float time)
    {
        //set start color
        SteamVR_Fade.Start(Color.black, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.clear, time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Died)
        {
            Died = true;
            FadeToBlack(0.2f);
            StartCoroutine(resetScene(1f));
        }
    }
    IEnumerator resetScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Main");
    }

    private void FixedUpdate()
    {
        if (GameStarted)
        {
            playerRB.velocity = ((Player.forward * CurrentbaseSpeed) + (LeftHandDirection * CurrentLeftSpeed) + (RightHandDirection * CurrentRightSpeed));
        }
    }
}