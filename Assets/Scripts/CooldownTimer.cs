using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CooldownTimer
{
    private float _cdTime;
    private float _timeStamp;

    public CooldownTimer(float time, bool startReady = true)
    {
        _cdTime = time;

        if(!startReady)
            _timeStamp = Time.time + _cdTime;
    }

    public bool IsReady()
    {
        if(_timeStamp <= Time.time)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Reset()
    {
        _timeStamp = Time.time + _cdTime;
    }
}
