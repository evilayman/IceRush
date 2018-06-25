using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    private ParticleSystem rippleOut;
    private ParticleSystem rippleIn;
    private PhotonView photonView;
    private Vector3 playerPos;
    [SerializeField]
    private GameObject teleportPS;
    [SerializeField]
    private GameObject firstTeleportPSOthers;
    [SerializeField]
    private GameObject secondTeleportPSOthers;
    private float teleportDist;

    private void Start()
    {
        StartCoroutine(GetTeleportDistance());
        photonView = GetComponentInParent<PhotonView>();
    }


    public void TeleportPlayer(GameObject player,float teleportDistance)
    {
        playerPos = player.transform.position;
        playerPos.z += teleportDistance;
        player.transform.position = playerPos;      
    }

    public void StartPlayingTeleportPS()
    {
        Vector3 teleportPSPosition = new Vector3();
        teleportPS.SetActive(true);
        teleportPSPosition = teleportPS.transform.position;
        teleportPSPosition.z += teleportDist;
        teleportPS.transform.position = teleportPSPosition;
        teleportPS.GetComponent<ParticleSystem>().Play();
    }

    public void StopPlayingTeleportPS()
    {
        Vector3 teleportPSPosition = new Vector3();
        teleportPSPosition = teleportPS.transform.position;
        teleportPSPosition.z -= teleportDist;
        teleportPS.transform.position = teleportPSPosition;
        teleportPS.SetActive(false);
    }

    public void PlayFirstTeleportPSForOthers(GameObject player)
    {
        photonView.RPC("RPC_PlayFirstPSOthers", PhotonTargets.Others,player.transform.position);
    }

    public void PlaySecondTeleportPSForOthers(GameObject player)
    {
        photonView.RPC("RPC_PlaySecondPSOthers", PhotonTargets.Others, player.transform.position);
    }

    [PunRPC]
    private void RPC_PlayFirstPSOthers(Vector3 playerPos)
    {
        print("First PS is playing");
        teleportPS.SetActive(true);
        rippleIn = firstTeleportPSOthers.GetComponentInChildren<ParticleSystem>();
        firstTeleportPSOthers.transform.position = playerPos;
        rippleIn.Play();
    }

    [PunRPC]
    private void RPC_PlaySecondPSOthers(Vector3 playerPos)
    {
        teleportPS.SetActive(true);
        rippleOut = secondTeleportPSOthers.GetComponentInChildren<ParticleSystem>();
        secondTeleportPSOthers.transform.position = playerPos;
        rippleOut.Play();
    }

    IEnumerator GetTeleportDistance()
    {
        yield return new WaitForSeconds(1);
        teleportDist = GetComponent<UsePowerUp>().TeleportDistance;
    }

    
}
