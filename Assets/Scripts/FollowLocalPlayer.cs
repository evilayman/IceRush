using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowLocalPlayer : MonoBehaviour
{
    public Transform target;
    private PhotonView photonView;
    private GameManager GM;

	void Start ()
    {
        photonView = GetComponent<PhotonView>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
	}
	
	void Update ()
    {
        if (photonView.isMine || GM.Offline)
        {
            target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
        }
    }
}
