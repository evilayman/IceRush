using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using VRTK;
using TMPro;

public class UsePowerUp : MonoBehaviour
{
    private VRTK_ControllerEvents leftHandController, rightHandController;
    private PlayerManagerForNetwork playerManager;
    private TeleportScript teleport;
    public TextMeshPro powerUpText;
    [SerializeField]
    float teleportDistance, boostTime;
    private bool teleportPSIsPlaying, tempBool;
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
    private PowerUpType lastPower;

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

    private bool showingPower;

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

        if (lastPower != currentPower && photonView.isMine)
        {
            lastPower = currentPower;
            showingPower = true;
            StartCoroutine(ShowPower());
        }

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

        if ((Input.GetKeyUp(KeyCode.Space) || !rightHandController.gripPressed) && photonView.isMine)
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

        if (leftHandController.gripPressed && photonView.isMine)
        {
            StartCoroutine(ShowPower());
        }
    }

    IEnumerator ShowPower()
    {
        powerUpText.text = CurrentPower.ToString();
        yield return new WaitForSeconds(1.5f);
        powerUpText.text = "";

    }

    private void UsePower()
    {
        switch (currentPower)
        {
            case PowerUpType.Rocket:
                myPower = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Rocket"), transform.GetChild(0).position, Quaternion.identity, 0);
                break;
            case PowerUpType.Sheild:
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