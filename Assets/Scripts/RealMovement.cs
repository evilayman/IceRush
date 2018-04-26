using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RealMovement : MonoBehaviour
{
    public MoveLeftHand leftHandScript;
    public MoveRightHand rightHandScript;
    private Rigidbody playerRB;

    public Transform LeftHandTrigger, RightHandTrigger, Player;

    public float HandPushSpeed, PlayerSpeed, AccSpeed, AccTime, DirThreshold;
    private float CurrentPlayerSpeed, CurrentLeftSpeed, CurrentRightSpeed, LeftHandSpeed, RightHandSpeed, tempLeft, tempRight;

    private Vector3 LeftHandDirection, RightHandDirection, oldDirLeft, oldDirRight;

    private bool isAccelerating = false, isAcceleratingLeft = false, isAcceleratingRight = false,
        GameStarted = false, Died = false;

    

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        FadeFromBlack(0.2f);

    }

    void Update()
    {
        if (leftHandScript.IsPressedLeft)
        {
            
            LeftHandDirection = - LeftHandTrigger.forward;
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
            LeftHandSpeed = 0;
            if (!isAcceleratingLeft && CurrentLeftSpeed > LeftHandSpeed)
            {
                isAcceleratingLeft = true;
                StartCoroutine(AccelerateLeft());
            }
        }

        if (rightHandScript.IsPressedRight)
        {
            RightHandDirection = - RightHandTrigger.forward;
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
            RightHandSpeed = 0;
            if (!isAcceleratingRight && CurrentRightSpeed > RightHandSpeed)
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
            playerRB.velocity = ((Player.forward * CurrentPlayerSpeed) + (LeftHandDirection * CurrentLeftSpeed) + (RightHandDirection * CurrentRightSpeed));
            //if(playerRB.velocity.magnitude < (HandPushSpeed*2))
            //    playerRB.AddForce((Player.forward * CurrentPlayerSpeed) + (LeftHandDirection * CurrentLeftSpeed) + (RightHandDirection * CurrentRightSpeed));

            tempRight = (oldDirRight - RightHandDirection).magnitude;
            tempLeft = (oldDirLeft - LeftHandDirection).magnitude;

            if (tempRight >= DirThreshold)
            {
                oldDirRight = RightHandDirection;
                print("Changed Right");
            }

            if (tempLeft >= DirThreshold)
            {
                oldDirLeft = LeftHandDirection;
                print("Changed Left");
            }
        }
    }
}