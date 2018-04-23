using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindersDistance : MonoBehaviour
{

    
    private void OnTriggerEnter(Collider other)
    {
        Vector3 ObstaclePosition = other.GetComponent<GameObject>().transform.position;
    }

    void Start ()
    {
		
	}
	
	void Update ()
    {
		
	}
}
