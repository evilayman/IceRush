using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPresScript : MonoBehaviour
{
    private Rigidbody rb;
    private Transform target;
    public float speed, rotSpeed;
	void Start ()
    {
        target = FindObjectOfType<MoveTarget>().transform;
        rb = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate ()
    {

        Vector3 targetDir = target.position - transform.position;

        float step = rotSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);

        transform.rotation = Quaternion.LookRotation(newDir);

        rb.velocity = transform.forward * speed;
    }
}
