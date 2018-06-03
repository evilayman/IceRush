using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBoundries : MonoBehaviour
{
    public Transform boundries;
    private PhotonView photonView;
    private GameManager GM;
    private Transform top, bot, left, right, front, back;
    private bool outBoundsSides, outBoundsTop, outBoundsBot, outBoundsFront, outBoundsBack;

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        photonView = GetComponent<PhotonView>();

        if (photonView.isMine || GM.Offline)
            Init();
    }

    private void Init()
    {
        top = boundries.GetChild(0).transform;
        bot = boundries.GetChild(1).transform;

        left = boundries.GetChild(2).transform;
        right = boundries.GetChild(3).transform;

        front = boundries.GetChild(4).transform;
        back = boundries.GetChild(5).transform;

    }

    void Update()
    {
        if (photonView.isMine || GM.Offline)
        {
            outBoundsSides = (transform.position.x < left.position.x || transform.position.x > right.position.x) ? true : false;

            outBoundsTop = (transform.position.y > top.position.y) ? true : false;
            outBoundsBot = (transform.position.y < bot.position.y) ? true : false;

            outBoundsFront = (transform.position.z > front.position.z) ? true : false;
            outBoundsBack = (transform.position.z < back.position.z) ? true : false;

            if (outBoundsSides)
                Debug.Log("Out Sides");
            if (outBoundsTop)
                Debug.Log("Out Top");
            if (outBoundsBot)
                Debug.Log("Out Bot");
            if (outBoundsFront)
                Debug.Log("Out Front");
            if (outBoundsBack)
                Debug.Log("Out Back");

        }
    }
}
