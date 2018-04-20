using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForce : MonoBehaviour
{
    private Rigidbody playerRB;

    public Transform LeftHandTrigger, RightHandTrigger, Player;

    public float HandVelocity, PlayerSpeed, TiltSpeed;
    private float LeftVelocity, RightVelocity;
    private Vector3 LeftHand, RightHand;

    private bool isPressedLeft = false, isPressedRight = false, NothingPressed = true;
    public bool IsPressedLeft
    {
        get
        {
            return isPressedLeft;
        }

        set
        {
            isPressedLeft = value;
        }
    }
    public bool IsPressedRight
    {
        get
        {
            return isPressedRight;
        }

        set
        {
            isPressedRight = value;
        }
    }


    void Start ()
    {
        playerRB = GetComponent<Rigidbody>();
    }
	
	void Update ()
    {
        if (IsPressedLeft)
            LeftVelocity = HandVelocity;
        else
            LeftVelocity = 0;

        if (IsPressedRight)
            RightVelocity = HandVelocity;
        else
            RightVelocity = 0;

    }

    private void FadeToBlack()
    {
        //set start color
        SteamVR_Fade.Start(Color.clear, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.black, 0.5f);
    }
    private void FadeFromBlack()
    {
        //set start color
        SteamVR_Fade.Start(Color.black, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.clear, 0.5f);
    }

    private void FixedUpdate()
    {
        //playerRB.velocity = (Player.forward * PlayerSpeed) + (new Vector3(0,-LeftHandTrigger.forward.y,0) * LeftVelocity) + (new Vector3(0, -RightHandTrigger.forward.y, 0) * RightVelocity);
        playerRB.velocity = ((Player.forward * PlayerSpeed) + (-LeftHandTrigger.forward * LeftVelocity) + (-RightHandTrigger.forward * RightVelocity));
        //playerRB.AddForce((Player.forward * PlayerSpeed) + (-LeftHandTrigger.forward * LeftVelocity) + (-RightHandTrigger.forward * RightVelocity));
        //Debug.Log(playerRB.velocity.z);

    }
}
