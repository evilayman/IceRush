using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLines : MonoBehaviour
{

    public ParticleSystem speedLineParticles;
    public float minRBSpeed, maxRBSpeed, minLineRate, maxLineRate, minRadius, maxRadius;
    private Rigidbody playerRigidBody;


    void Start()
    {
        playerRigidBody = GetComponent<Rigidbody>();

    }


    void Update()
    {
        if (playerRigidBody.velocity.z + 1 >= minRBSpeed)
        {
            speedLineParticles.Play();
            var emission = speedLineParticles.emission;
            emission.rateOverTime = (playerRigidBody.velocity.z - minRBSpeed) * ((maxLineRate - minLineRate) / (maxRBSpeed - minRBSpeed)) + minLineRate;
            var shape = speedLineParticles.shape;
            shape.radius = (playerRigidBody.velocity.z - minRBSpeed) * ((maxRadius - minRadius) / (maxRBSpeed - minRBSpeed)) + minRadius;

        }
        else
        {
            speedLineParticles.Stop();
        }


    }
}
