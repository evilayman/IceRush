using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLocalPlayer : MonoBehaviour
{
    public Transform targetNormal, targetLerp;
    public float lerpT;
    private PhotonView photonView;
    private GameManager GM;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void FixedUpdate()
    {
        if (photonView.isMine || GM.Offline)
        {
            if (targetNormal)
                targetNormal.transform.position = new Vector3(targetNormal.transform.position.x, targetNormal.transform.position.y, transform.position.z);

            if (targetLerp)
            {
                var targetLerpPos = new Vector3(targetLerp.transform.position.x, targetLerp.transform.position.y, transform.position.z);
                targetLerp.transform.position = Vector3.Lerp(targetLerp.transform.position, targetLerpPos, lerpT * Time.fixedDeltaTime);
            }
        }

    }
}
