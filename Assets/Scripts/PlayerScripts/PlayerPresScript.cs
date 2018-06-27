using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresScript : MonoBehaviour
{

	void Start ()
    {
		
	}
	
	void FixedUpdate ()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * 10;
	}
}
