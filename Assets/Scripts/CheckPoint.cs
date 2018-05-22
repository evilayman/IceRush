using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private Transform spawnPoint;
    // Use this for initialization
    void Start()
    {
        spawnPoint = GetComponent<Transform>().GetChild(0);
    }
    private void OnTriggerEnter(Collider other)
    {
        print(other.tag);
        if (other.tag=="Player")
        {
            other.GetComponent<PlayerManagerForNetwork>().spawnPoint = spawnPoint;
            print("done" + other.GetComponent<PlayerManagerForNetwork>().spawnPoint);
        }
    }
}
