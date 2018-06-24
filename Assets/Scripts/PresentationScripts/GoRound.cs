using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoRound : MonoBehaviour
{
    public float rotSpeed, speed;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * rotSpeed);
        transform.Translate(Vector3.forward * speed);
    }
}
