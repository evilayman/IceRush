using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardFollow : MonoBehaviour
{
    public Transform head;
	
	void FixedUpdate ()
    {
        transform.position = new Vector3(head.transform.position.x, transform.position.y, head.transform.position.z);
	}
}
