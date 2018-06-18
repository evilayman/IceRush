using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRegion : MonoBehaviour
{
    public float speed, regionSize, stopPoint;

    void Update()
    {
        if (transform.position.z < stopPoint)
            transform.position += Vector3.forward * speed * Time.deltaTime;
    }
}
