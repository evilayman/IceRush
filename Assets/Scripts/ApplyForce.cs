using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplyForce : MonoBehaviour
{
    private Rigidbody playerRB;

    public Transform LeftHandTrigger, RightHandTrigger, Player;

    public float HandVelocity, PlayerSpeed;
    private float LeftVelocity, RightVelocity;
    private Vector3 LeftHand, RightHand;

    private bool isPressedLeft = false, isPressedRight = false, GameStarted = false, Died = false;

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
        FadeFromBlack(0.2f);
    }
	
	void Update ()
    {
        if (IsPressedLeft)
        {
            LeftVelocity = HandVelocity;
            GameStarted = true;
        }
        else
            LeftVelocity = 0;

        if (IsPressedRight)
        { 
            RightVelocity = HandVelocity;
            GameStarted = true;
        }
        else
            RightVelocity = 0;

    }

    private void FadeToBlack(float time)
    {
        //set start color
        SteamVR_Fade.Start(Color.clear, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.black, time);
    }
    private void FadeFromBlack(float time)
    {
        //set start color
        SteamVR_Fade.Start(Color.black, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.clear, time);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!Died)
        {
            Died = true;
            FadeToBlack(0.2f);
            StartCoroutine(resetScene(1f));
        }
        
    }

    IEnumerator resetScene(float time)
    { 
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Main");
    }

    private void FixedUpdate()
    {
        if(GameStarted)
        {
            //playerRB.velocity = (Player.forward * PlayerSpeed) + (new Vector3(0,-LeftHandTrigger.forward.y,0) * LeftVelocity) + (new Vector3(0, -RightHandTrigger.forward.y, 0) * RightVelocity);
            playerRB.velocity = ((Player.forward * PlayerSpeed) + (-LeftHandTrigger.forward * LeftVelocity) + (-RightHandTrigger.forward * RightVelocity));
            //playerRB.AddForce((Player.forward * PlayerSpeed) + (-LeftHandTrigger.forward * LeftVelocity) + (-RightHandTrigger.forward * RightVelocity));
            //Debug.Log(playerRB.velocity.z);
        }


    }
}
