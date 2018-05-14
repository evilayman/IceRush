using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRegion : MonoBehaviour
{
    public float speed, regionSize;
	
	void Update ()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }
}
