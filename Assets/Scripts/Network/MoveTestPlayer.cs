using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTestPlayer : Photon.MonoBehaviour
{

    private Vector3 selfPos;

    void Start()
    {
    }

    void Update()
    {
        if (photonView.isMine)
        {
            MovePlayer();
        }
        else
        {
            SmoothNetMovement();
        }

    }

    private void MovePlayer()
    {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Time.deltaTime * 10;
    }

    private void SmoothNetMovement()
    {
        transform.position = Vector3.Lerp(transform.position, selfPos, Time.deltaTime * 8);
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.isWriting)
        {
            stream.SendNext(transform.position);
        }
        else
        {
            selfPos = (Vector3)stream.ReceiveNext();
        }
    }
}
