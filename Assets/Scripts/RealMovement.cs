using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RealMovement : MonoBehaviour
{
    public HandScript leftHandScript, rightHandScript;

    private Rigidbody playerRB;

    public Transform LeftHandTrigger, RightHandTrigger;

    public float PlayerSpeed, HandPushSpeed, AccTime, AccSpeed, DirThreshold;
    private float CurrentPlayerSpeed;

    private Vector3 LeftHandDirection, RightHandDirection;

    public bool GameStarted = false;
    private bool isAccelerating = false, isAcceleratingLeft = false, isAcceleratingRight = false, Died = false;
    
    private List<Vector3> myDirectionsLeft, myDirectionsRight;
    private List<float> myDirSpeedLeft, myDirSpeedRight;

    public ParticleSystem EmissionLeft, EmissionRight;

    private void OnCollisionEnter(Collision collision)
    {
        if (!Died)
        {
            Died = true;
            FadeToBlack(0.2f);
            StartCoroutine(ResetScene(1f));
        }
    }

    void Start()
    {
        init();
        FadeFromBlack(0.2f);
    }

    void init()
    {
        playerRB = GetComponent<Rigidbody>();
        myDirectionsLeft = myDirectionsRight = new List<Vector3>();
        myDirSpeedLeft = myDirSpeedRight = new List<float>();
    }

    void Update()
    {
        CheckBaseSpeed();

        CheckLeftHand();
        CheckRightHand();
    }

    private void FixedUpdate()
    {
        if (GameStarted)
            MovePlayer();
    }

    void CheckBaseSpeed()
    {
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
    }

    void CheckHand(bool isTriggerPressed, Vector3 handDirection, Transform HandTrigger, ref List<Vector3> myDirections, ref List<float> myDirSpeeds)
    {

        if (isTriggerPressed)
        {
            handDirection = -HandTrigger.forward;
            GetDirs(ref myDirections, ref myDirSpeeds, handDirection);

            if (!isAcceleratingLeft)
            {
                isAcceleratingLeft = true;

                for (int i = 0; i < myDirSpeedLeft.Count; i++)
                {
                    if (i == myDirSpeedLeft.Count - 1)
                    {
                        StartCoroutine(AccelerateHand(myDirSpeedLeft[i], HandPushSpeed));
                    }
                    else
                    {
                        StartCoroutine(AccelerateHand(myDirSpeedLeft[i], 0));
                    }
                }
            }

            if (!GameStarted)
                GameStarted = true;
        }
        else
        {
            if (!isAcceleratingLeft)
            {
                isAcceleratingLeft = true;

                for (int i = 0; i < myDirSpeedLeft.Count; i++)
                {
                    StartCoroutine(AccelerateHand(myDirSpeedLeft[i], 0));
                }
            }
        }
    }
    void CheckLeftHand()
    {
        if (leftHandScript.IsTriggerPressed)
        {
            if (!EmissionLeft.isPlaying)
                EmissionLeft.Play();

            LeftHandDirection = -LeftHandTrigger.forward;
            GetDirsLeft();

            if (!isAcceleratingLeft)
            {
                isAcceleratingLeft = true;

                for (int i = 0; i < myDirSpeedLeft.Count; i++)
                {
                    if (i == myDirSpeedLeft.Count - 1)
                    {
                        StartCoroutine(AccelerateLeft(i, HandPushSpeed));
                    }
                    else
                    {
                        StartCoroutine(AccelerateLeft(i, 0));
                    }
                }
            }

            if (!GameStarted)
                GameStarted = true;
        }
        else
        {

            if (EmissionLeft.isPlaying)
                EmissionLeft.Stop();

            if (!isAcceleratingLeft)
            {
                isAcceleratingLeft = true;

                for (int i = 0; i < myDirSpeedLeft.Count; i++)
                {
                    StartCoroutine(AccelerateLeft(i, 0));
                }
            }
        }
    }
    void CheckRightHand()
    {
        if (!EmissionRight.isPlaying)
            EmissionRight.Play();

        if (rightHandScript.IsTriggerPressed)
        {
            RightHandDirection = -RightHandTrigger.forward;
            GetDirsRight();

            if (!isAcceleratingRight)
            {
                isAcceleratingRight = true;

                for (int i = 0; i < myDirSpeedRight.Count; i++)
                {
                    if (i == myDirSpeedRight.Count - 1)
                    {
                        StartCoroutine(AccelerateRight(i, HandPushSpeed));
                    }
                    else
                    {
                        StartCoroutine(AccelerateRight(i, 0));
                    }
                }
            }

            if (!GameStarted)
                GameStarted = true;
        }
        else
        {
            if (EmissionRight.isPlaying)
                EmissionRight.Stop();

            if (!isAcceleratingRight)
            {
                isAcceleratingRight = true;

                for (int i = 0; i < myDirSpeedRight.Count; i++)
                {
                    StartCoroutine(AccelerateRight(i, 0));
                }
            }
        }
    }

    void GetDirs(ref List<Vector3> MyDirections, ref List<float> myDirSpeeds, Vector3 handDirection)
    {
        if (MyDirections.Count == 0)
        {
            MyDirections.Add(handDirection);
            myDirSpeeds.Add(0);
        }

        var temp = (MyDirections[MyDirections.Count - 1] - handDirection).magnitude;

        if (temp >= DirThreshold)
        {
            MyDirections.Add(handDirection);
            myDirSpeeds.Add(0);
        }
    }
    void GetDirsLeft()
    {
        if(myDirectionsLeft.Count == 0)
        {
            myDirectionsLeft.Add(LeftHandDirection);
            myDirSpeedLeft.Add(0);
        }
        
        float temp = (myDirectionsLeft[myDirectionsLeft.Count - 1] - LeftHandDirection).magnitude;

        if (temp >= DirThreshold)
        {
            myDirectionsLeft.Add(LeftHandDirection);
            myDirSpeedLeft.Add(0);
        }
    }
    void GetDirsRight()
    {
        if (myDirectionsRight.Count == 0)
        {
            myDirectionsRight.Add(RightHandDirection);
            myDirSpeedRight.Add(0);
        }

        float temp = (myDirectionsRight[myDirectionsRight.Count - 1] - RightHandDirection).magnitude;

        if (temp >= DirThreshold)
        {
            myDirectionsRight.Add(RightHandDirection);
            myDirSpeedRight.Add(0);
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

    IEnumerator AccelerateHand(float currentSpeed, float targetSpeed)
    {
        yield return new WaitForSeconds(AccTime);

        if (currentSpeed < targetSpeed)
        {
            currentSpeed += AccSpeed;
        }
        else if (currentSpeed > targetSpeed)
        {
            currentSpeed -= AccSpeed;
        }

        isAcceleratingLeft = false;
    }
    IEnumerator AccelerateLeft(int SpeedIndex, float TargetSpeed)
    {
        yield return new WaitForSeconds(AccTime);

        if (myDirSpeedLeft[SpeedIndex] < TargetSpeed)
        {
            myDirSpeedLeft[SpeedIndex] += AccSpeed;
        }
        else if (myDirSpeedLeft[SpeedIndex] > TargetSpeed)
        {
            myDirSpeedLeft[SpeedIndex] -= AccSpeed;
        }

        isAcceleratingLeft = false;
    }
    IEnumerator AccelerateRight(int SpeedIndex, float TargetSpeed)
    {
        yield return new WaitForSeconds(AccTime);

        if (myDirSpeedRight[SpeedIndex] < TargetSpeed)
        {
            myDirSpeedRight[SpeedIndex] += AccSpeed;
        }
        else if (myDirSpeedRight[SpeedIndex] > TargetSpeed)
        {
            myDirSpeedRight[SpeedIndex] -= AccSpeed;
        }

        isAcceleratingRight = false;
    }

    void MovePlayer()
    {
        //playerRB.velocity = ((Player.forward * CurrentPlayerSpeed) + (LeftHandDirection * CurrentLeftSpeed) + (RightHandDirection * CurrentRightSpeed));
        //if(playerRB.velocity.magnitude < (HandPushSpeed*2))
        //    playerRB.AddForce((Player.forward * CurrentPlayerSpeed) + (LeftHandDirection * CurrentLeftSpeed) + (RightHandDirection * CurrentRightSpeed));

        LeftVelocity();
        RightVelocity();
    }

    void LeftVelocity()
    {
        for (int i = 0; i < myDirectionsLeft.Count; i++)
        {
            if (myDirSpeedLeft[i] == 0)
            {
                myDirectionsLeft.Remove(myDirectionsLeft[i]);
                myDirSpeedLeft.Remove(myDirSpeedLeft[i]);
            }
            else
                playerRB.velocity += (myDirectionsLeft[i] * myDirSpeedLeft[i]);
        }
    }
    void RightVelocity()
    {
        for (int i = 0; i < myDirectionsRight.Count; i++)
        {
            if (myDirSpeedRight[i] == 0)
            {
                myDirectionsRight.Remove(myDirectionsRight[i]);
                myDirSpeedRight.Remove(myDirSpeedRight[i]);
            }
            else
                playerRB.velocity += (myDirectionsRight[i] * myDirSpeedRight[i]);
        }
    }

    private void FadeToBlack(float time)
    {
        //set start color
        SteamVR_Fade.Start(Color.clear, 0f);
        //set and start fade tos
        SteamVR_Fade.Start(Color.black, time);
    }
    private void FadeFromBlack(float time)
    {
        //set start color
        SteamVR_Fade.Start(Color.black, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.clear, time);
    }
    IEnumerator ResetScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Main");
    }
}