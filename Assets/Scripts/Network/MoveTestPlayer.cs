using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTestPlayer : Photon.MonoBehaviour
{
    private Rigidbody playerRB;
    private Vector3 targetPos;
    private float lag;

    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (photonView.isMine)
            MovePlayer();
    }

    private void FixedUpdate()
    {
        if (!photonView.isMine)
            SmoothNetMovement();
    }

    private void MovePlayer()
    {
        playerRB.velocity += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * 30;
    }

    private void SmoothNetMovement()
    {
        playerRB.position = Vector3.MoveTowards(playerRB.position, targetPos, Time.fixedDeltaTime);
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(playerRB.position);
            stream.SendNext(playerRB.velocity);
        }
        else
        {
            targetPos = (Vector3)stream.ReceiveNext();
            playerRB.velocity = (Vector3)stream.ReceiveNext();

            lag = Mathf.Abs((float)(PhotonNetwork.time - info.timestamp));
            targetPos += (this.playerRB.velocity * lag);
        }
    }
}
