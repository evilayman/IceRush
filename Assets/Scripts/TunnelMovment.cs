using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMovment : MonoBehaviour
{
    public float force;
    private Collider col;
    // Use this for initialization
    void Start()
    {
        if (gameObject.GetComponent<BoxCollider>() == null)
            col = gameObject.AddComponent<BoxCollider>();

        col = gameObject.GetComponent<BoxCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.transform.forward = gameObject.transform.forward;
            
                
        }
    }
}
