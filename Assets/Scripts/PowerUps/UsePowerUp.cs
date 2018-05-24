using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UsePowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        None,
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

    private PhotonView photonView;
    private GameObject myPower;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && photonView.isMine)
        {
            UsePower();
        }
    }

    private void UsePower()
    {
        switch (currentPower)
        {
            case PowerUpType.Rocket:
                myPower = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Rocket"), transform.GetChild(0).position, Quaternion.identity, 0);
                break;
            case PowerUpType.Sheild:
                break;
            case PowerUpType.Boost:
                break;
            case PowerUpType.Trap:
                break;
            case PowerUpType.Teleport:
                break;
            default:
                break;
        }
        currentPower = PowerUpType.None;
    }
}