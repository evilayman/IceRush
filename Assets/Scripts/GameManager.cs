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

    private List<GameObject> myPlayersSorted, finishedPlayers;
    private List<float> finishTime;
    public List<GameObject> MyPlayersSorted
    {
        get
        {
            return myPlayersSorted;
        }

        set
        {
            myPlayersSorted = value;
        }
    }

    private bool inGameFirstTime, endGameFirstTime;

    private void Start()
    {
        MyPlayersSorted = new List<GameObject>();
        finishedPlayers = new List<GameObject>();
        finishTime = new List<float>();
    }

    void AddPlayers()
    {
        var players = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < players.Length; i++)
        {
            MyPlayersSorted.Add(players[i].transform.GetChild(0).gameObject);
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.none:
                break;
            case GameState.preGame:
                break;
            case GameState.inGame:
                if (!inGameFirstTime)
                {
                    inGameFirstTime = true;
                    AddPlayers();
                }
                MyPlayersSorted.Sort(SortByDistance);
                CheckFinishLine();
                break;
            case GameState.goalReached:
                MyPlayersSorted.Sort(SortByDistance);
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
            finishedPlayers.Add(myPlayersSorted[0]);
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
        if (MyPlayersSorted.Count > 0 && (myPlayersSorted[0].transform.position.z - finishLine.position.z) >= 0)
        {
            if (currentState != GameState.goalReached)
                currentState = GameState.goalReached;

            myPlayersSorted[0].GetComponentInParent<PlayerTextManager>().ReachGoal = true;

            finishedPlayers.Add(myPlayersSorted[0]);
            finishTime.Add(Time.time);

            myPlayersSorted.RemoveAt(0);
        }
    }

    private int SortByDistance(GameObject A, GameObject B)
    {
        return -(A.transform.position.z - finishLine.position.z).CompareTo(B.transform.position.z - finishLine.position.z);
    }
}
