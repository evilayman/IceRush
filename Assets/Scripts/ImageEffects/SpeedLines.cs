using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    public Transform speedLines;

    public ParticleSystem speedLineParticles;

    private Rigidbody playerRB;

    public float minRBSpeed, maxRBSpeed, minLineRate, maxLineRate, minRadius, maxRadius, rotationSpeed;
    private float rbVelocity;

    private Vector3 newDir;




    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }


    void Update()
    {

        newDir = Vector3.RotateTowards(speedLines.forward, playerRB.velocity.normalized, rotationSpeed, 0f);
        speedLines.rotation = Quaternion.LookRotation(newDir);

        rbVelocity = playerRB.velocity.magnitude;

        if (rbVelocity + 1 >= minRBSpeed)
        {
            if (!speedLineParticles.isPlaying)
                speedLineParticles.Play();

            float rot = (rbVelocity - minRBSpeed) * ((maxLineRate - minLineRate) / (maxRBSpeed - minRBSpeed)) + minLineRate;
            var emission = speedLineParticles.emission;

            if (rot <= maxLineRate)
                emission.rateOverTime = rot;

            var rad = (rbVelocity - minRBSpeed) * ((maxRadius - minRadius) / (maxRBSpeed - minRBSpeed)) + minRadius;
            var shape = speedLineParticles.shape;

            if (rad >= maxRadius)
                shape.radius = rad;

        }
        else
        {
            if (speedLineParticles.isPlaying)
                speedLineParticles.Stop();
        }


    }
}
