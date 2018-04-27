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

    private Vector3 LeftHandDirection, RightHandDirection, totalSpeedLeft, totalSpeedRight;

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

        //CheckHand(leftHandScript.IsTriggerPressed, LeftHandDirection, LeftHandTrigger, ref myDirectionsLeft, ref myDirSpeedLeft, isAcceleratingLeft, (x) => isAcceleratingLeft = x);
        //CheckHand(rightHandScript.IsTriggerPressed, RightHandDirection, RightHandTrigger, ref myDirectionsRight, ref myDirSpeedRight, isAcceleratingRight, (x) => isAcceleratingRight = x);

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
    void CheckHand(bool isTriggerPressed, Vector3 handDirection, Transform HandTrigger, ref List<Vector3> myDirections, ref List<float> myDirSpeeds, bool isHandAcc, System.Action<bool> myAction)
    {
        if (isTriggerPressed)
        {
            handDirection = -HandTrigger.forward;
            GetDirs(ref myDirections, ref myDirSpeeds, handDirection);

            if (!isHandAcc)
            {
                isHandAcc = true;
                for (int i = 0; i < myDirSpeeds.Count; i++)
                {
                    if (i == myDirSpeeds.Count - 1 && myDirSpeeds[i] < HandPushSpeed)
                    {
                        myDirSpeeds[i] += AccSpeed;
                    }
                    else if(myDirSpeeds[i] > 0)
                    {
                        myDirSpeeds[i] -= AccSpeed;
                    }
                }
                StartCoroutine(AccelerateHand(myAction));
            }

            if (!GameStarted)
                GameStarted = true;
        }
        else
        {
            if (!isHandAcc)
            {
                isAcceleratingLeft = true;
                for (int i = 0; i < myDirSpeeds.Count; i++)
                {
                    if (myDirSpeeds[i] > 0)
                    {
                        myDirSpeeds[i] -= AccSpeed;
                    }
                }
                StartCoroutine(AccelerateHand(myAction));
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
                    if (i == myDirSpeedLeft.Count - 1 && myDirSpeedLeft[i] < HandPushSpeed)
                    {
                        myDirSpeedLeft[i] += AccSpeed;
                    }
                    else if (myDirSpeedLeft[i] > 0)
                    {
                        myDirSpeedLeft[i] -= AccSpeed;
                    }
                }

                StartCoroutine(AccelerateLeft());
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
                    myDirSpeedLeft[i] -= AccSpeed;
                }

                StartCoroutine(AccelerateLeft());
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
                    if (i == myDirSpeedRight.Count - 1 && myDirSpeedRight[i] < HandPushSpeed)
                    {
                        myDirSpeedRight[i] += AccSpeed;
                    }
                    else if (myDirSpeedRight[i] > 0)
                    {
                        myDirSpeedRight[i] -= AccSpeed;
                    }
                }

                StartCoroutine(AccelerateRight());

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
                    myDirSpeedRight[i] -= AccSpeed;
                }
                StartCoroutine(AccelerateRight());
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
    IEnumerator AccelerateHand(System.Action<bool> myAction)
    {
        yield return new WaitForSeconds(AccTime);
        myAction(false);
    }
    IEnumerator AccelerateLeft()
    {
        yield return new WaitForSeconds(AccTime);
        isAcceleratingLeft = false;
    }
    IEnumerator AccelerateRight()
    {
        yield return new WaitForSeconds(AccTime);
        isAcceleratingRight = false;
    }

    void MovePlayer()
    {
        //playerRB.velocity = ((Player.forward * CurrentPlayerSpeed) + (LeftHandDirection * CurrentLeftSpeed) + (RightHandDirection * CurrentRightSpeed));
        //if(playerRB.velocity.magnitude < (HandPushSpeed*2))
        //    playerRB.AddForce((Player.forward * CurrentPlayerSpeed) + (LeftHandDirection * CurrentLeftSpeed) + (RightHandDirection * CurrentRightSpeed));

        playerRB.velocity = LeftVelocity() + RightVelocity();
        //playerRB.velocity = HandVelocity(myDirectionsLeft, myDirSpeedLeft, totalSpeedLeft) + HandVelocity(myDirectionsRight, myDirSpeedRight, totalSpeedRight);

    }

    Vector3 HandVelocity(List<Vector3> myDirections, List<float> myDirSpeeds, Vector3 totalSpeed)
    {
        totalSpeed = Vector3.zero;
        for (int i = 0; i < myDirections.Count; i++)
        {
            if (myDirSpeeds[i] == 0)
            {
                myDirections.Remove(myDirections[i]);
                myDirSpeeds.Remove(myDirSpeeds[i]);
            }
            else
                totalSpeed += (myDirections[i] * myDirSpeeds[i]);
        }
        return totalSpeed;
    }
    Vector3 LeftVelocity()
    {
        totalSpeedLeft = Vector3.zero;
        for (int i = 0; i < myDirectionsLeft.Count; i++)
        {
            if (myDirSpeedLeft[i] == 0)
            {
                myDirectionsLeft.Remove(myDirectionsLeft[i]);
                myDirSpeedLeft.Remove(myDirSpeedLeft[i]);
            }
            else
                totalSpeedLeft += (myDirectionsLeft[i] * myDirSpeedLeft[i]);
        }
        return totalSpeedLeft;
    }
    Vector3 RightVelocity()
    {
        totalSpeedRight = Vector3.zero;
        for (int i = 0; i < myDirectionsRight.Count; i++)
        {
            if (myDirSpeedRight[i] == 0)
            {
                myDirectionsRight.Remove(myDirectionsRight[i]);
                myDirSpeedRight.Remove(myDirSpeedRight[i]);
            }
            else
                totalSpeedRight = (myDirectionsRight[i] * myDirSpeedRight[i]);
        }
        return totalSpeedRight;
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