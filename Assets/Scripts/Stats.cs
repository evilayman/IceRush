using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Stats : MonoBehaviour
{
    public VRTK_ControllerEvents leftHand, rightHand; 
    public Transform leftHandTransform, rightHandTransform;
    [Space(10)]
    public float baseSpeed;
    public float handSpeed, boostSpeed, maxSpeed;
    [Space(10)]
    public float decTime;
    public float decRate;
    [Space(10)]
    public float accTimeBoost;
    public float accRateBoost, decTimeBoost, decRateBoost;
    [Space(10)]
    public float accTimeHand;
    public float accRateHand, decTimeHand, decRateHand;


}
