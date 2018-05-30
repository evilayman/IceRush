using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        none,
        preGame,
        inGame,
        goalReached,
        endGame
    }

    public GameState currentState;
    public Transform finishLine;
    public TextMeshPro debugText;

    private List<GameObject> myPlayersSorted, finishedPlayers;
    private List<float> finishTime;

    private int playersLoaded, playerList;
    private float waitTime;
    private bool inGameFirstTime, endGameFirstTime, stopCheck, startWait, offline = true;

    public int PlayersLoaded
    {
        get
        {
            return playersLoaded;
        }

        set
        {
            playersLoaded = value;
        }
    }

    public bool Offline
    {
        get
        {
            return offline;
        }

        set
        {
            offline = value;
        }
    }

    private void Start()
    {
        MyPlayersSorted = new List<GameObject>();
        finishedPlayers = new List<GameObject>();
        finishTime = new List<float>();
        playerList = PhotonNetwork.playerList.Length;

        if (Offline)
        {
            currentState = GameState.inGame;
        }
    }

    [PunRPC]
    void RPC_playerLoaded()
    {
        PlayersLoaded += 1;
    }

    private void CheckPlayersCount()
    {
        if (PhotonNetwork.isMasterClient && !stopCheck)
        {
            if (PlayersLoaded == playerList)
            {
                stopCheck = true;
                gameObject.GetPhotonView().RPC("RPC_InitPreState", PhotonTargets.AllViaServer, (int)(PhotonNetwork.ServerTimestamp + 1000));
            }
        }
    }

    [PunRPC]
    void RPC_InitPreState(int time)
    {
        waitTime = time;
        startWait = true;
    }

    void AddPlayers()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            myPlayersSorted.Add(players[i].transform.GetChild(0).gameObject);
        }
    }

    private void WaitStart()
    {
        if (startWait)
        {
            if (PhotonNetwork.ServerTimestamp >= waitTime)
            {
                currentState = GameState.preGame;
                debugText.text = "";
                startWait = false;
            }
        }
    }
    private void Update()
    {
        CheckPlayersCount();
        WaitStart();

        switch (currentState)
        {
            case GameState.none:
                inGameFirstTime = false;
                break;
            case GameState.preGame:
                break;
            case GameState.inGame:
                if (!inGameFirstTime)
                {
                    inGameFirstTime = true;
                    AddPlayers();
                }
                myPlayersSorted.Sort(SortByDistance);
                CheckFinishLine();
                break;
            case GameState.goalReached:
                myPlayersSorted.Sort(SortByDistance);
                CheckFinishLine();
                break;
            case GameState.endGame:
                if (!endGameFirstTime)
                {
                    endGameFirstTime = true;
                    CompleteFinishList();
                    FinalScreen();
                }
                break;
            default:
                break;
        }
    }

    private void CompleteFinishList()
    {
        for (int i = 0; i < myPlayersSorted.Count; i++)
        {
            myPlayersSorted[i].GetComponentInParent<PlayerManagerForNetwork>().currentPlayerState = PlayerManagerForNetwork.PlayerState.SlowToStop;
            finishedPlayers.Add(myPlayersSorted[i]);
            finishTime.Add(0);
        }
    }

    private void FinalScreen()
    {
        for (int i = 0; i < finishedPlayers.Count; i++)
        {
            print(finishedPlayers[i].GetComponentInParent<PhotonView>().owner.NickName + " - Time: " + finishTime[i]);
        }
    }

    private void CheckFinishLine()
    {
        if (myPlayersSorted.Count > 0 && (myPlayersSorted[0].transform.position.z - finishLine.position.z) >= 0)
        {
            if (currentState != GameState.goalReached)
                currentState = GameState.goalReached;

            myPlayersSorted[0].GetComponentInParent<PlayerTextManager>().ReachGoal = true;
            myPlayersSorted[0].GetComponentInParent<PlayerManagerForNetwork>().currentPlayerState = PlayerManagerForNetwork.PlayerState.SlowToStop;

            finishedPlayers.Add(myPlayersSorted[0]);
            finishTime.Add(Time.time);

            myPlayersSorted.RemoveAt(0);
        }
    }

    public int GetRank(GameObject GO)
    {
        myPlayersSorted.FindIndex(x => x == GO);

        return myPlayersSorted.FindIndex(x => x == GO) + finishedPlayers.Count;
    }

    private int SortByDistance(GameObject A, GameObject B)
    {
        return -(A.transform.position.z - finishLine.position.z).CompareTo(B.transform.position.z - finishLine.position.z);
    }
}
