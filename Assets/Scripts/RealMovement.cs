using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RealMovement : MonoBehaviour
{
    
    public HandScript leftHandScript, rightHandScript;

    private Rigidbody playerRB;

    public Transform leftHandTrigger, rightHandTrigger;

    public float baseSpeed, handSpeed, accTime, accRate, decRate, accTimeHand, accRateHand, decRateHand, dirThreshold;
    private float currentBaseSpeed;

    private Vector3 leftHandDirection, rightHandDirection, totalSpeedLeft, totalSpeedRight;

    public bool gameStarted = false;
    private bool isAccelerating = false, isAcceleratingLeft = false, isAcceleratingRight = false, Died = false;
    
    public List<Vector3> myDirectionsLeft, myDirectionsRight;
    public List<float> myDirSpeedLeft, myDirSpeedRight;

    public ParticleSystem emissionLeft, emissionRight;

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
        CheckbaseSpeed();

        //CheckHand(leftHandScript.IsTriggerPressed, leftHandDirection, leftHandTrigger, ref myDirectionsLeft, ref myDirSpeedLeft, isAcceleratingLeft, (x) => isAcceleratingLeft = x);
        //CheckHand(rightHandScript.IsTriggerPressed, rightHandDirection, rightHandTrigger, ref myDirectionsRight, ref myDirSpeedRight, isAcceleratingRight, (x) => isAcceleratingRight = x);

        CheckLeftHand();
        CheckRightHand();
    }

    private void FixedUpdate()
    {
        if (gameStarted)
            MovePlayer();
    }

    void CheckbaseSpeed()
    {
        if (!isAccelerating && currentBaseSpeed < baseSpeed)
        {
            isAccelerating = true;
            StartCoroutine(Accelerate(accTime));
        }
        else if (!isAccelerating && currentBaseSpeed > baseSpeed)
        {
            isAccelerating = true;
            StartCoroutine(Accelerate(accTime));
        }
    }
    void CheckHand(bool isTriggerPressed, Vector3 handDirection, Transform HandTrigger, ref List<Vector3> myDirections, ref List<float> myDirSpeeds, bool isHandAcc, System.Action<bool> myAction)
    {
        if (isTriggerPressed)
        {
            handDirection = HandTrigger.forward;
            GetDirs(ref myDirections, ref myDirSpeeds, handDirection);

            if (!isHandAcc)
            {
                isHandAcc = true;
                for (int i = 0; i < myDirSpeeds.Count; i++)
                {
                    if (i == myDirSpeeds.Count - 1 && myDirSpeeds[i] < handSpeed)
                    {
                        myDirSpeeds[i] += accRateHand;
                    }
                    else if(myDirSpeeds[i] > 0)
                    {
                        myDirSpeeds[i] -= decRateHand;
                    }
                }
                StartCoroutine(AccelerateHand(myAction, accTimeHand));
            }

            if (!gameStarted)
                gameStarted = true;
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
                        myDirSpeeds[i] -= decRateHand;
                    }
                }
                StartCoroutine(AccelerateHand(myAction, accTimeHand));
            }
        }
    }
    void CheckLeftHand()
    {
        if (leftHandScript.IsTriggerPressed)
        {
            if (!emissionLeft.isPlaying)
                emissionLeft.Play();

            leftHandDirection = leftHandTrigger.forward;
            GetDirsLeft();

            if (!isAcceleratingLeft)
            {
                isAcceleratingLeft = true;

                for (int i = 0; i < myDirSpeedLeft.Count; i++)
                {
                    if (i == myDirSpeedLeft.Count - 1 && myDirSpeedLeft[i] < handSpeed)
                    {
                        myDirSpeedLeft[i] += accRateHand;
                    }
                    else if (myDirSpeedLeft[i] > 0)
                    {
                        myDirSpeedLeft[i] -= decRateHand;
                    }
                }

                StartCoroutine(AccelerateLeft(accTimeHand));
            }

            if (!gameStarted)
                gameStarted = true;
        }
        else
        {

            if (emissionLeft.isPlaying)
                emissionLeft.Stop();

            if (!isAcceleratingLeft)
            {
                isAcceleratingLeft = true;

                for (int i = 0; i < myDirSpeedLeft.Count; i++)
                {
                    myDirSpeedLeft[i] -= decRateHand;
                }

                StartCoroutine(AccelerateLeft(accTimeHand));
            }
        }
    }
    void CheckRightHand()
    {
        if (rightHandScript.IsTriggerPressed)
        {
            if (!emissionRight.isPlaying)
                emissionRight.Play();

            rightHandDirection = rightHandTrigger.forward;
            GetDirsRight();

            if (!isAcceleratingRight)
            {
                isAcceleratingRight = true;

                for (int i = 0; i < myDirSpeedRight.Count; i++)
                {
                    if (i == myDirSpeedRight.Count - 1 && myDirSpeedRight[i] < handSpeed)
                    {
                        myDirSpeedRight[i] += accRateHand;
                    }
                    else if (myDirSpeedRight[i] > 0)
                    {
                        myDirSpeedRight[i] -= decRateHand;
                    }
                }

                StartCoroutine(AccelerateRight(accTimeHand));

            }

            if (!gameStarted)
                gameStarted = true;
        }
        else
        {
            if (emissionRight.isPlaying)
                emissionRight.Stop();

            if (!isAcceleratingRight)
            {
                isAcceleratingRight = true;

                for (int i = 0; i < myDirSpeedRight.Count; i++)
                {
                    myDirSpeedRight[i] -= decRateHand;
                }

                StartCoroutine(AccelerateRight(accTimeHand));
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

        if (temp >= dirThreshold)
        {
            MyDirections.Add(handDirection);
            myDirSpeeds.Add(0);
        }
    }

    void GetDirsLeft()
    {
        if(myDirectionsLeft.Count == 0)
        {
            myDirectionsLeft.Add(leftHandDirection);
            myDirSpeedLeft.Add(0);
        }
        
        float temp = (myDirectionsLeft[myDirectionsLeft.Count - 1] - leftHandDirection).magnitude;

        if (temp >= dirThreshold)
        {
            myDirectionsLeft.Add(leftHandDirection);
            myDirSpeedLeft.Add(myDirSpeedLeft[myDirectionsLeft.Count]);
        }
    }
    void GetDirsRight()
    {
        if (myDirectionsRight.Count == 0)
        {
            myDirectionsRight.Add(rightHandDirection);
            myDirSpeedRight.Add(0);
        }

        float temp = (myDirectionsRight[myDirectionsRight.Count - 1] - rightHandDirection).magnitude;

        if (temp >= dirThreshold)
        {
            myDirectionsRight.Add(rightHandDirection);
            myDirSpeedRight.Add(myDirSpeedRight[myDirectionsRight.Count]);
        }
    }

    IEnumerator Accelerate(float time)
    {
        yield return new WaitForSeconds(time);

        if (currentBaseSpeed < baseSpeed)
        {
            currentBaseSpeed += accRate;
        }
        else if (currentBaseSpeed > baseSpeed)
        {
            currentBaseSpeed -= decRate;
        }
        isAccelerating = false;
    }
    IEnumerator AccelerateHand(System.Action<bool> myAction, float time)
    {
        yield return new WaitForSeconds(time);
        myAction(false);
    }
    IEnumerator AccelerateLeft(float time)
    {
        yield return new WaitForSeconds(time);
        isAcceleratingLeft = false;
    }
    IEnumerator AccelerateRight(float time)
    {
        yield return new WaitForSeconds(time);
        isAcceleratingRight = false;
    }

    void MovePlayer()
    {
        //playerRB.velocity = ((Player.forward * currentBaseSpeed) + (leftHandDirection * CurrentLeftSpeed) + (rightHandDirection * CurrentRightSpeed));
        //if(playerRB.velocity.magnitude < (handSpeed*2))
        //    playerRB.AddForce((Player.forward * currentBaseSpeed) + (leftHandDirection * CurrentLeftSpeed) + (rightHandDirection * CurrentRightSpeed));

        playerRB.velocity = LeftVelocity() + RightVelocity() + (transform.forward * currentBaseSpeed);
        //playerRB.velocity = HandVelocity(myDirectionsLeft, myDirSpeedLeft, totalSpeedLeft) + HandVelocity(myDirectionsRight, myDirSpeedRight, totalSpeedRight);

    }

    Vector3 HandVelocity(List<Vector3> myDirections, List<float> myDirSpeeds, Vector3 totalSpeed)
    {
        totalSpeed = Vector3.zero;
        for (int i = 0; i < myDirections.Count; i++)
        {
            if (myDirSpeeds[i] <= 0)
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
            if (myDirSpeedLeft[i] <= 0)
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
            if (myDirSpeedRight[i] <= 0)
            {
                myDirectionsRight.Remove(myDirectionsRight[i]);
                myDirSpeedRight.Remove(myDirSpeedRight[i]);
            }
            else
                totalSpeedRight += (myDirectionsRight[i] * myDirSpeedRight[i]);
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