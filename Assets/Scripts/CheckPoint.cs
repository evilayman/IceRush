using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = GetComponent<Transform>().GetChild(0).position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollider")
        {
            other.GetComponentInParent<PlayerManagerForNetwork>().SpawnPoint = spawnPoint;
        }
    }
}
