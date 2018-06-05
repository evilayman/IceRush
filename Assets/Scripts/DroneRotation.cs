using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneRotation : MonoBehaviour
{
    private float rotationSpeed = 50;

    void Update()
    {
        transform.Rotate(new Vector3(1, 1, 1) * (rotationSpeed * Time.deltaTime));
    }
}
