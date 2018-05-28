using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMovment : MonoBehaviour
{
    public float moveTowardsSpeed;
    public float maxRaduisDelta;
    public List<Transform> posList;
    public float collisionCheckDistance;
    public List<Transform> PosList
    {
        get
        {
            return posList;
        }

        set
        {
            posList = value;
        }
    }
    private BoxCollider col;
    private Rigidbody rb;



    // Use this for initialization
    void Start()
    {
        col = gameObject.GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

    }
   

    private void Update()
    {
        RaycastHit hit;
        if (rb.SweepTest(-transform.forward, out hit, collisionCheckDistance))
        {
            print("Collided");
        }
    }

    public void AddPoint()
    {
        var go = new GameObject("Point");
        go.transform.SetParent(transform);
        go.transform.position = new Vector3(0, 50, 0);
        go.transform.localScale = new Vector3(0, 0, 0);
        PosList.Add(go.transform);
    }
}
