using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeadTextManager : MonoBehaviour
{
    public TextMeshPro playerName, playerRank;

    [Space(10)]
    public float minDistance;
    public float maxDistance, minRankFontSize, maxRankFontSize;

    private GameManager GM;
    private PhotonView photonView;
    private CooldownTimer countDownTimer;
    private bool reachGoal, countStartDone = true, countFinishDone = true;
    private Transform localPlayer;
    private float rankFontSize, ratio;

    public bool ReachGoal
    {
        get
        {
            return reachGoal;
        }

        set
        {
            reachGoal = value;
        }
    }

    void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        FindLocalPlayer();
    }


    void FindLocalPlayer()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < players.Length; i++)
        {
            if (PhotonNetwork.player.ID == players[i].GetPhotonView().owner.ID)
            {
                localPlayer = players[i].transform;
                break;
            }
        }
    }

    private void Update()
    {
        setRankText();
        rotateText();
    }

    private void rotateText()
    {
        if (Camera.main)
            playerName.transform.rotation = playerRank.transform.rotation = Camera.main.transform.rotation;
    }

    private void setRankText()
    {
        playerRank.SetText(GetRankString(GM.GetRank(transform.GetChild(0).gameObject)));

        var distance = Vector3.Distance(localPlayer.position, transform.position);

        var rankFontSize = (distance - minDistance) * ((maxRankFontSize - minRankFontSize) / (maxDistance - minDistance)) + minRankFontSize;

        if (rankFontSize > maxRankFontSize)
            rankFontSize = maxRankFontSize;
        else if (rankFontSize < minRankFontSize)
            rankFontSize = minRankFontSize;

        playerRank.fontSize = rankFontSize;
    }

    private string GetRankString(int index)
    {
        string rank;
        switch (index)
        {
            case 0:
                rank = "1st";
                break;
            case 1:
                rank = "2nd";
                break;
            case 2:
                rank = "3rd";
                break;
            default:
                rank = "";
                break;
        }
        return rank;
    }
}

