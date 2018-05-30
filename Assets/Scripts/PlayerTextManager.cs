using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTextManager : MonoBehaviour
{
    public TextMeshPro playerName, playerRank, playerCounter;

    public float countDownTimeStart, countDownTimeFinish, timerCD;
    public int fontSize, maxFontSize, fontSizeIncrementSpeed;

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
        photonView = GetComponent<PhotonView>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (!photonView.isMine && !GM.Offline)
        {
            playerName.SetText(photonView.owner.NickName);
            FindLocalPlayer();
        }
        else
        {
            countDownTimer = new CooldownTimer(timerCD);
            playerName.gameObject.SetActive(false);
            playerRank.gameObject.SetActive(false);
        }
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
        if (!photonView.isMine && !GM.Offline)
        {
            setRankText();
            rotateText();
        }
        else
        {
            if (GM.currentState == GameManager.GameState.preGame)
            {
                countStartDone = false;
            }
            else if (GM.currentState == GameManager.GameState.goalReached)
            {
                countFinishDone = false;
            }

            if (!countStartDone)
                CountDown(ref countDownTimeStart, "Go!", GameManager.GameState.inGame, ref countStartDone);

            if (!countFinishDone)
                CountDown(ref countDownTimeFinish, "Game Over", GameManager.GameState.endGame, ref countFinishDone);


            if (Camera.main && playerCounter)
                playerCounter.transform.rotation = Camera.main.transform.rotation;
        }
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

    private void CountDown(ref float countDownTime, string finalWord, GameManager.GameState nextState, ref bool done)
    {
        if (countDownTime > -1)
        {
            if (countDownTime <= 0)
            {
                if (!ReachGoal)
                    playerCounter.text = finalWord;
                else
                    playerCounter.text = "";

                if (GM.currentState != nextState)
                    GM.currentState = nextState;
            }
            else
            {
                if (!ReachGoal)
                    playerCounter.text = countDownTime.ToString();
                else
                    playerCounter.text = "";
            }

            if (countDownTime <= 3 && playerCounter.fontSize < maxFontSize)
            {
                playerCounter.fontSize += fontSizeIncrementSpeed;
            }

            if (countDownTimer.IsReady())
            {
                countDownTimer.Reset();
                countDownTime -= timerCD;

                if (countDownTime >= 0)
                    playerCounter.fontSize = fontSize;
            }
        }
        else
        {
            playerCounter.text = "";
            done = true;
        }
    }
}