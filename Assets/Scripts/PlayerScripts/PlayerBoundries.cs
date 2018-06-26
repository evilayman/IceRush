using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerBoundries : MonoBehaviour
{
    public TextMeshPro boundsText;

    private GameManager GM;
    private PlayerManagerForNetwork PM;
    private PhotonView photonView;
    private Transform boundries;
    private Transform top, bot, left, right, front, back;
    private bool outBounds, outBoundsSides, outBoundsTop, outBoundsBot, outBoundsFront, outBoundsBack;
    private Coroutine co;
    private AudioManager AM;

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        AM = FindObjectOfType<AudioManager>();
        boundries = GameObject.Find("Boundries").transform;
        photonView = GetComponent<PhotonView>();

        if (photonView.isMine || GM.Offline)
            Init();
    }

    private void Init()
    {
        PM = GetComponent<PlayerManagerForNetwork>();

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
            BoundsCheck();
        }
    }

    void BoundsCheck()
    {
        outBoundsSides = (transform.position.x < left.position.x || transform.position.x > right.position.x) ? true : false;

        outBoundsTop = (transform.position.y > top.position.y) ? true : false;
        outBoundsBot = (transform.position.y < bot.position.y) ? true : false;

        outBoundsFront = (transform.position.z > front.position.z) ? true : false;
        outBoundsBack = (transform.position.z < back.position.z) ? true : false;

        if (outBoundsTop || outBoundsBot || outBoundsFront || outBoundsBack || outBoundsSides)
        {
            if (!outBounds)
            {
                outBounds = true;
                PM.currentPlayerState = PlayerManagerForNetwork.PlayerState.Slowed;
                co = StartCoroutine(CountDownBounds(5));
            }
        }
        else
        {
            if (outBounds)
            {
                outBounds = false;
                PM.currentPlayerState = PlayerManagerForNetwork.PlayerState.Normal;
                StopCoroutine(co);
                boundsText.text = "";
            }
        }

        //if (outBoundsTop)
        //    boundsText.text = "Out Top\n";
        //else if (outBoundsBot)
        //    boundsText.text = "Out Bot\n";
        //else if (outBoundsFront)
        //    boundsText.text = "Out Front\n";
        //else if (outBoundsBack)
        //    boundsText.text = "Out Back\n";
        //else if (outBoundsSides)
        //    boundsText.text = "Out Sides\n";
        //else
        //    boundsText.text = "";
    }

    private IEnumerator CountDownBounds(int countDownTime)
    {
        var waitTime = new WaitForSeconds(1);
        for (int i = countDownTime; i > 0; i--)
        {
            boundsText.text = "Out of Bounds \n" + i.ToString();
            yield return waitTime;
        }
        AM.Play("Freezing", Instantiate(new GameObject(), transform));
        boundsText.text = "Frozen"; 
        yield return waitTime;
        gameObject.GetPhotonView().RPC("RPC_Collision", PhotonTargets.All);
        boundsText.text = "";
    }
}
