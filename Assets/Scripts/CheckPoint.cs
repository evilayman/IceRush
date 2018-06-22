using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Transform spawnPoint;

    void Start()
    {
        spawnPoint = GetComponent<Transform>().GetChild(0);

        //GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        //foreach (var player in players)
        //{
        //    player.GetComponentInParent<PlayerManagerForNetwork>().SpawnPoint = spawnPoint;
        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "PlayerCollider")
    //    {
    //        other.GetComponentInParent<PlayerManagerForNetwork>().SpawnPoint = spawnPoint;
    //    }
    //}
}
