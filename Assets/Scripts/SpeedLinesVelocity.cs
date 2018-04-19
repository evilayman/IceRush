using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLinesVelocity : MonoBehaviour
{

    public ParticleSystem speedLineParticles;
    public float lineSpeed,lineDistance;
    private Rigidbody playerRigidBody;


    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();

    }


    void Update()
    {
        if (playerRigidBody.velocity.z>0)
        {
            speedLineParticles.Play();
            speedLineParticles.GetComponentInParent<Transform>().localPosition = new Vector3(0, 0, playerRigidBody.velocity.z * lineDistance);
            var t = speedLineParticles.velocityOverLifetime;
            t.speedModifier = playerRigidBody.velocity.z / lineSpeed;
        }
        else
        {
            speedLineParticles.Stop();
        }
        
     
    }
}
