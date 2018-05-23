using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPowerUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "PlayerParent")
        {
            other.gameObject.GetComponent<UsePowerUp>().CurrentPower = UsePowerUp.PowerUpType.Rocket;
        }
    }


}
