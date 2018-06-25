using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using VRTK;

public class UsePowerUp : MonoBehaviour
{
    private VRTK_ControllerEvents leftHandController, rightHandController;
    private PlayerManagerForNetwork playerManager;
    private TeleportScript teleport;
    [SerializeField]
    float teleportDistance, boostTime;
    private bool teleportPSIsPlaying,tempBool;
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

    public float TeleportDistance
    {
        get
        {
            return teleportDistance;
        }

        set
        {
            teleportDistance = value;
        }
    }

    private PhotonView photonView;
    private GameObject myPower;

    private void Start()
    {
        playerManager = GetComponent<PlayerManagerForNetwork>();
        teleport = (TeleportScript)FindObjectOfType(typeof(TeleportScript));
        photonView = GetComponent<PhotonView>();

        leftHandController = playerManager.leftHand.GetComponent<VRTK_ControllerEvents>();
        rightHandController = playerManager.rightHand.GetComponent<VRTK_ControllerEvents>();
    }
    void Update()
    {
      
        //if ((Input.GetKeyUp(KeyCode.Space) && photonView.isMine ))
        //{
        //    if (CurrentPower == PowerUpType.Teleport)
        //    {
        //        teleport.PlayFirstTeleportPSForOthers(gameObject);               
        //        UsePower();
        //        teleport.PlaySecondTeleportPSForOthers(gameObject);
        //        teleport.StopPlayingTeleportPS();               
        //        teleportPSIsPlaying = false;
        //    }
        //}
        //if ((Input.GetKeyDown(KeyCode.Space) || rightHandController.gripPressed) && photonView.isMine)
        //{
        //    if (CurrentPower != PowerUpType.Teleport)
        //        UsePower();
        //    if (CurrentPower == PowerUpType.Teleport && !teleportPSIsPlaying)
        //    {
        //        teleportPSIsPlaying = true;
        //        teleport.StartPlayingTeleportPS();
        //    }
        //}
      
        if ((Input.GetKeyUp(KeyCode.Space ) || !rightHandController.gripPressed) && photonView.isMine)
        {
            
            if (CurrentPower == PowerUpType.Teleport && tempBool)
            {
                teleport.PlayFirstTeleportPSForOthers(gameObject);
                UsePower();
                teleport.PlaySecondTeleportPSForOthers(gameObject);
                teleport.StopPlayingTeleportPS();
                teleportPSIsPlaying = false;
                tempBool = false;
            }
            
        }
        if ((Input.GetKeyDown(KeyCode.Space) || rightHandController.gripPressed) && photonView.isMine)
        {
            if (CurrentPower != PowerUpType.Teleport)
                UsePower();
            if (CurrentPower == PowerUpType.Teleport && !teleportPSIsPlaying)
            {
                tempBool = true;
                teleportPSIsPlaying = true;
                teleport.StartPlayingTeleportPS();
            }
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
                playerManager.StartCoroutine("BoostPlayer", new object[] { boostTime });
                break;
            case PowerUpType.Trap:
                break;
            case PowerUpType.Teleport:
                teleport.TeleportPlayer(gameObject, teleportDistance);
                break;
            default:
                break;
        }
        currentPower = PowerUpType.None;
    }
}