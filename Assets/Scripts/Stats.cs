using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Stats")]
public class Stats : ScriptableObject
{
    [Header("Common Stats")]
    [Space(10)]
    public float baseSpeed;
    public float maxSpeed;
    [Space(10)]
    public float decTime;
    public float decRate;
    [Space(10)]
    public float boostSpeed;
    public float accTimeBoost, accRateBoost, decTimeBoost, decRateBoost;
    [Space(10)]
    [Header("Hand Stats")]
    [Space(10)]
    public float handSpeed;
    public float accTimeHand, accRateHand, decTimeHand, decRateHand;
    [Space(10)]
    [Header("Lever Stats")]
    [Space(10)]
    public float leverStepSpeed;
    public float accTimeLever, accRateLever, decTimeLever, decRateLever;
}
