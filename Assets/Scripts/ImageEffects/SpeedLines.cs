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

            float rot = (playerRigidBody.velocity.z - minRBSpeed) * ((maxLineRate - minLineRate) / (maxRBSpeed - minRBSpeed)) + minLineRate;
            var emission = speedLineParticles.emission;
            emission.rateOverTime = rot;

            var rad = (playerRigidBody.velocity.z - minRBSpeed) * ((maxRadius - minRadius) / (maxRBSpeed - minRBSpeed)) + minRadius;
            var shape = speedLineParticles.shape;

            if(rad >= maxRadius)
                shape.radius = rad;

        }
        else
        {
            speedLineParticles.Stop();
        }


    }
}
