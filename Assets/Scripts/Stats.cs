using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public HandScript leftHandScript, rightHandScript;
    public Transform leftHandTrigger, rightHandTrigger;
    [Space(10)]
    public float baseSpeed;
    public float boostSpeed, handSpeed;
    [Space(10)]
    public float accTime;
    public float accRate, decRate;
    [Space(10)]
    public float accTimeHand;
    public float accRateHand, decRateHand;
    [Space(10)]
    public float dirThreshold;

}
