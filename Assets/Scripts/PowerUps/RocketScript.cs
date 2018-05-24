using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    public Vector3 startPosition;
    public float rocketSpeed, rocketRotSpeed, rocketDestructTime;

    private Transform player, target, hitPlayer;
    private GameManager GM;
    private Rigidbody RB;
    private Vector3 dir;
    private PhotonView photonView;

    void Start()
    {
        transform.position = transform.position + startPosition;
        RB = GetComponent<Rigidbody>();
        photonView = gameObject.GetComponent<PhotonView>();

        FindOwner();

        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        var i = GM.MyPlayersSorted.FindIndex(x => x == player.GetChild(0).gameObject);

        if (i != 0)
            target = GM.MyPlayersSorted[i - 1].transform;

        if(photonView.isMine)
            StartCoroutine(AutoDestroyRocket(rocketDestructTime));
    }

    void FindOwner()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if(gameObject.GetPhotonView().owner.ID == players[i].GetPhotonView().owner.ID)
            {
                player = players[i].transform;
                break;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(photonView.isMine && collision.transform != player)
        {
            if (collision.gameObject.tag == "Player")
            {
                PhotonNetwork.Destroy(gameObject);
                collision.gameObject.GetPhotonView().RPC("RPC_Collision", PhotonTargets.All);
            }
            else
            {
                PhotonNetwork.Destroy(gameObject);
            }
        }
    }

    void FixedUpdate()
    {
        SearchAndDestroy();
    }

    private void SearchAndDestroy()
    {
        if (target)
        {
            dir = (target.position - transform.position).normalized;
        }
        else
        {
            dir = Vector3.forward;
        }

        RB.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, dir, rocketRotSpeed, 0.0f));
        RB.velocity = transform.forward * rocketSpeed;
    }

    IEnumerator AutoDestroyRocket(float time)
    {
        yield return new WaitForSeconds(time);

        if (gameObject)
            PhotonNetwork.Destroy(gameObject);
    }
}
