using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealMovement : MonoBehaviour
{

    private Stats myStats;

    private Rigidbody playerRB;

    private float currentBaseSpeed, availableSpeedLeft, availableSpeedRight;

    private Vector3 leftHandDirection, rightHandDirection, totalSpeedLeft, totalSpeedRight;

    public bool gameStarted = false;

    private bool isAccelerating = false, isAcceleratingLeft = false, isAcceleratingRight = false, Died = false;
    
    private List<Vector3> myDirectionsLeft, myDirectionsRight;
    private List<float> myDirSpeedLeft, myDirSpeedRight;

    public ParticleSystem emissionLeft, emissionRight;

    void Start()
    {
        init();
    }

    private void init()
    {
        myStats = gameObject.GetComponent<Stats>();
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
        if (!isAccelerating && currentBaseSpeed < myStats.baseSpeed)
        {
            isAccelerating = true;
            StartCoroutine(Accelerate(myStats.accTime));
        }
        else if (!isAccelerating && currentBaseSpeed > myStats.baseSpeed)
        {
            isAccelerating = true;
            StartCoroutine(Accelerate(myStats.accTime));
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
                    if (i == myDirSpeeds.Count - 1 && myDirSpeeds[i] < myStats.handSpeed)
                    {
                        myDirSpeeds[i] += myStats.accRateHand;
                    }
                    else if(myDirSpeeds[i] > 0)
                    {
                        myDirSpeeds[i] -= myStats.decRateHand;
                    }
                }
                StartCoroutine(AccelerateHand(myAction, myStats.accTimeHand));
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
                        myDirSpeeds[i] -= myStats.decRateHand;
                    }
                }
                StartCoroutine(AccelerateHand(myAction, myStats.accTimeHand));
            }
        }
    }
    void CheckLeftHand()
    {
        if (myStats.leftHandScript.IsTriggerPressed)
        {
            if (!emissionLeft.isPlaying)
                emissionLeft.Play();

            leftHandDirection = myStats.leftHandTrigger.forward;

            for (int i = 0; i < myDirSpeedLeft.Count; i++)
            {
                availableSpeedLeft += myDirSpeedLeft[i];
            }

            availableSpeedLeft = myStats.handSpeed - availableSpeedLeft;

            GetDirsLeft();

            if (!isAcceleratingLeft)
            {
                isAcceleratingLeft = true;

                for (int i = 0; i < myDirSpeedLeft.Count; i++)
                {
                    if (i == myDirSpeedLeft.Count - 1 && myDirSpeedLeft[i] < availableSpeedLeft)
                    {
                        myDirSpeedLeft[i] += myStats.accRateHand;
                    }
                    else if (myDirSpeedLeft[i] > 0)
                    {
                        myDirSpeedLeft[i] -= myStats.decRateHand;
                    }
                }

                StartCoroutine(AccelerateLeft(myStats.accTimeHand));
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
                    myDirSpeedLeft[i] -= myStats.decRateHand;
                }

                StartCoroutine(AccelerateLeft(myStats.accTimeHand));
            }
        }
    }
    void CheckRightHand()
    {
        if (myStats.rightHandScript.IsTriggerPressed)
        {
            if (!emissionRight.isPlaying)
                emissionRight.Play();

            rightHandDirection = myStats.rightHandTrigger.forward;

            for (int i = 0; i < myDirSpeedRight.Count; i++)
            {
                availableSpeedRight += myDirSpeedRight[i];
            }

            availableSpeedRight = myStats.handSpeed - availableSpeedRight;

            GetDirsRight();

            if (!isAcceleratingRight)
            {
                isAcceleratingRight = true;

                for (int i = 0; i < myDirSpeedRight.Count; i++)
                {
                    if (i == myDirSpeedRight.Count - 1 && myDirSpeedRight[i] < availableSpeedRight)
                    {
                        myDirSpeedRight[i] += myStats.accRateHand;
                    }
                    else if (myDirSpeedRight[i] > 0)
                    {
                        myDirSpeedRight[i] -= myStats.decRateHand;
                    }
                }

                StartCoroutine(AccelerateRight(myStats.accTimeHand));

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
                    myDirSpeedRight[i] -= myStats.decRateHand;
                }

                StartCoroutine(AccelerateRight(myStats.accTimeHand));
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

        if (temp >= myStats.dirThreshold)
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

        if (temp >= myStats.dirThreshold)
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

        if (temp >= myStats.dirThreshold)
        {
            myDirectionsRight.Add(rightHandDirection);
            myDirSpeedRight.Add(myDirSpeedRight[myDirectionsRight.Count]);
        }
    }

    IEnumerator Accelerate(float time)
    {
        yield return new WaitForSeconds(time);

        if (currentBaseSpeed < myStats.baseSpeed)
        {
            currentBaseSpeed += myStats.accRate;
        }
        else if (currentBaseSpeed > myStats.baseSpeed)
        {
            currentBaseSpeed -= myStats.decRate;
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
}