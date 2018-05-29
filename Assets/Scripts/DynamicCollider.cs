using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicCollider : MonoBehaviour
{
    public Transform headset;
    public CapsuleCollider myCollider;
    public float baseHeadHeight, baseColHeight, baseColCenter;

    void Update()
    {
        setCollider();
    }

    void setCollider()
    {
        myCollider.height = ( (headset.position.y - transform.position.y) * baseColHeight) / baseHeadHeight;
        
        myCollider.center = new Vector3(0, ((headset.position.y - transform.position.y) * baseColCenter) / baseHeadHeight, 0);
    }
}
