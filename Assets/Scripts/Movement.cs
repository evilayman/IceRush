using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    private Rigidbody playerRB;

    public Transform LeftHandTrigger, RightHandTrigger, Player;

    public float HandPushSpeed, PlayerSpeed, AccSpeed, AccTime;
    private float CurrentPlayerSpeed, CurrentLeftSpeed, CurrentRightSpeed;

    private Vector3 LeftHand, RightHand, LeftHandDirection, RightHandDirection;

    private bool isAccelerating = false, isAcceleratingLeft = false, isAcceleratingRight = false,
        isPressedLeft = false, isPressedRight = false, GameStarted = true, Died = false;

    public bool IsPressedLeft
    {
        get
        {
            return isPressedLeft;
        }

        set
        {
            isPressedLeft = value;
        }
    }
    public bool IsPressedRight
    {
        get
        {
            return isPressedRight;
        }

        set
        {
            isPressedRight = value;
        }
    }


    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        FadeFromBlack(0.2f);
    }

    void Update()
    {
        if (IsPressedLeft)
        {

            LeftHandDirection = -LeftHandTrigger.forward;

            if (!isAcceleratingLeft && CurrentLeftSpeed < HandPushSpeed)
            {
                isAcceleratingLeft = true;
                StartCoroutine(AccelerateLeft());
            }


            if (!GameStarted)
                GameStarted = true;
        }
        else
        {
            if (!isAcceleratingLeft && CurrentLeftSpeed > 0)
            {
                isAcceleratingLeft = true;
                StartCoroutine(AccelerateLeft());
            }
        }

        if (IsPressedRight)
        {

            RightHandDirection = -RightHandTrigger.forward;

            if (!isAcceleratingRight && CurrentRightSpeed < HandPushSpeed)
            {
                isAcceleratingRight = true;
                StartCoroutine(AccelerateRight());
            }

            if (!GameStarted)
                GameStarted = true;
        }
        else
        {
            if (!isAcceleratingRight && CurrentRightSpeed > 0)
            {
                isAcceleratingRight = true;
                StartCoroutine(AccelerateRight());
            }
        }

        if (!isAccelerating && CurrentPlayerSpeed < PlayerSpeed)
        {
            isAccelerating = true;
            StartCoroutine(Accelerate());
        }
        else if (!isAccelerating && CurrentPlayerSpeed > PlayerSpeed)
        {
            isAccelerating = true;
            StartCoroutine(Accelerate());
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerSpeed += 100;
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            PlayerSpeed -= 100;
        }
    }

    IEnumerator Accelerate()
    {
        yield return new WaitForSeconds(AccTime);

        if (CurrentPlayerSpeed < PlayerSpeed)
        {
            CurrentPlayerSpeed += AccSpeed;
        }
        else if (CurrentPlayerSpeed > PlayerSpeed)
        {
            CurrentPlayerSpeed -= AccSpeed;
        }
        isAccelerating = false;
    }

    IEnumerator AccelerateLeft()
    {
        yield return new WaitForSeconds(AccTime);

        if (CurrentLeftSpeed < HandPushSpeed)
        {
            CurrentLeftSpeed += AccSpeed;
        }
        else if (CurrentLeftSpeed > 0)
        {
            CurrentLeftSpeed -= AccSpeed;
        }
        isAcceleratingLeft = false;
    }

    IEnumerator AccelerateRight()
    {
        yield return new WaitForSeconds(AccTime);

        if (CurrentRightSpeed < HandPushSpeed)
        {
            CurrentRightSpeed += AccSpeed;
        }
        else if (CurrentRightSpeed > 0)
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
            //if (IsPressedRight && IsPressedLeft)
            {
                playerRB.velocity = ((Player.forward * CurrentPlayerSpeed) + (LeftHandDirection * CurrentLeftSpeed) + (RightHandDirection * CurrentRightSpeed));
            }
            //else if (IsPressedRight)
            //{
            //    playerRB.velocity = ((Player.forward * CurrentPlayerSpeed) + (-RightHandTrigger.forward * CurrentRightSpeed));
            //}
            //else if (IsPressedLeft)
            //{
            //    playerRB.velocity = ((Player.forward * CurrentPlayerSpeed) + (-LeftHandTrigger.forward * CurrentLeftSpeed));
            //}
            //else
            //{
            //    playerRB.velocity = (Player.forward * CurrentPlayerSpeed);
             Debug.Log(playerRB.velocity.z);
            //}
        }
    }
}