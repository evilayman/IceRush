using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        none,
        Rocket,
        Sheild,
        Boost,
        Trap,
        Teleport
    }

    private PowerUpType currentPower;

    public PowerUpType CurrentPower
    {
        get
        {
            return currentPower;
        }

        set
        {
            currentPower = value;
        }
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
