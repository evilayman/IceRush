using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    private Transform player;

    public Vector3 startPosition;
    public float rocketSpeed, rocketTime;

    private Transform target;
    private GameManager GM;
    private Rigidbody RB;
    private Vector3 dir;


    void Start()
    {
        transform.position = transform.position + startPosition;
        RB = GetComponent<Rigidbody>();

        FindOwner();

        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        var i = GM.MyPlayersSorted.FindIndex(x => x == player.GetChild(0).gameObject);

        if (i != 0)
            target = GM.MyPlayersSorted[i - 1].transform;

        StartCoroutine(AutoDestroyRocket(rocketTime));
    }

    void FindOwner()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            if(gameObject.GetPhotonView().owner.ID == players[i].GetPhotonView().owner.ID)
            {
                player = players[i].transform;
                return;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "PlayerCollider")
        {
            PhotonNetwork.Destroy(gameObject);
            StartCoroutine(player.transform.GetComponentInParent<PlayerManagerForNetwork>().ReturnToLastRespawnPoint());
        }
        else
        {
            PhotonNetwork.Destroy(gameObject);
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

        RB.velocity = dir * rocketSpeed;
    }

    IEnumerator AutoDestroyRocket(float time)
    {
        yield return new WaitForSeconds(time);

        if (gameObject)
            PhotonNetwork.Destroy(gameObject);
    }
}
