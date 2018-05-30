using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UsePowerUp : MonoBehaviour
{
    private TeleportScript teleport;
    [SerializeField]
    float teleportDistance;
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
        teleport = new TeleportScript();
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
                print("Used Rocket");
                myPower = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Rocket"), transform.GetChild(0).position, Quaternion.identity, 0);
                break;
            case PowerUpType.Sheild:
                print("Used Shield");
                gameObject.transform.Find("ShieldInnerChild").gameObject.SetActive(true);
                break;
            case PowerUpType.Boost:
                break;
            case PowerUpType.Trap:
                break;
            case PowerUpType.Teleport:
                print("used teleport");
                teleport.TeleportPlayer(gameObject, teleportDistance);
                break;
            default:
                break;
        }
        currentPower = PowerUpType.None;
    }
}