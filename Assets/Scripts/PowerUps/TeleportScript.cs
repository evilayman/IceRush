using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(GetTeleportDistance());
    }
    private Vector3 playerPos;
    [SerializeField]
    private GameObject TeleportPS;
    private float teleportDist;
    public void TeleportPlayer(GameObject player,float teleportDistance)
    {
        playerPos = player.transform.position;
        playerPos.z += teleportDistance;
        player.transform.position = playerPos;
        
    }

    public void StartPlayingTeleportPS()
    {
        Vector3 teleportPSPosition = new Vector3();
        TeleportPS.SetActive(true);
        teleportPSPosition = TeleportPS.transform.position;
        teleportPSPosition.z += teleportDist;
        TeleportPS.transform.position = teleportPSPosition;
        TeleportPS.GetComponent<ParticleSystem>().Play();
    }

    public void StopPlayingTeleportPS()
    {
        Vector3 teleportPSPosition = new Vector3();
        teleportPSPosition = TeleportPS.transform.position;
        teleportPSPosition.z -= teleportDist;
        TeleportPS.transform.position = teleportPSPosition;
        TeleportPS.SetActive(false);
    }
   
    IEnumerator GetTeleportDistance()
    {
        yield return new WaitForSeconds(1);
        teleportDist = GetComponent<UsePowerUp>().TeleportDistance;
    }
}
