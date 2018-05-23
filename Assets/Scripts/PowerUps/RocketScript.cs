using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketScript : MonoBehaviour
{

    private Transform player;
    public Transform Player
    {
        get
        {
            return player;
        }

        set
        {
            player = value;
        }
    }

    public float rocketSpeed, rocketTime;

    private Transform target;
    private GameManager GM;

    void Start ()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        var i = GM.MyPlayersSorted.FindIndex(x => x == player.gameObject);

        if(i != 0)
            target = GM.MyPlayersSorted[i - 1].transform;

        StartCoroutine(AutoDestroyRocket(rocketTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {

        }
    }

    void Update ()
    {
		
	}

    IEnumerator AutoDestroyRocket(float time)
    {
        yield return new WaitForSeconds(time);

        if(gameObject)
            PhotonNetwork.Destroy(gameObject);
    }
}
