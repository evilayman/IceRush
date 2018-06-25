using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedLines : MonoBehaviour
{
    public Transform speedLines;

    public ParticleSystem speedLineParticles;

    private Rigidbody playerRB;
    private PhotonView photonView;


    public float minRBSpeed, maxRBSpeed, minLineRate, maxLineRate, minRadius, maxRadius;
    private float rbVelocity;

    private Vector3 newDir;
    private GameManager GM;
    private AudioManager AM;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = FindObjectOfType<AudioManager>();
        if (photonView.isMine || GM.Offline)
            playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (photonView.isMine || GM.Offline)
        {
            if(playerRB.velocity.magnitude > 30)
            {
                AM.Play("Fly", gameObject);
            }
            else
            {
                AM.Stop(gameObject);
            }

            if (playerRB.velocity != Vector3.zero)
            {
                speedLines.rotation = Quaternion.LookRotation(playerRB.velocity);
            }
            
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
}
