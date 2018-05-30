using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
    private Vector3 playerPos;

    public void TeleportPlayer(GameObject player,float teleportDistance)
    {
        playerPos = player.transform.position;
        playerPos.z += teleportDistance;
        player.transform.position = playerPos;
    }
}
