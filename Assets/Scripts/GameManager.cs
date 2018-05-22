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
        endGame
    }

    public GameState currentState;

    public float countDownTime, timerCD;
    public int fontSize, maxFontSize, fontSizeIncrementSpeed;

    public TextMeshPro countDownTimeText;
    public Transform finishLine;

    private CooldownTimer countDownTimer;
    private List<GameObject> myPlayersSorted;

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

    private void Start()
    {
        countDownTimer = new CooldownTimer(timerCD, false);
        MyPlayersSorted = new List<GameObject>();
        AddPlayers();

    }

    void AddPlayers()
    {
        var players = GameObject.FindGameObjectsWithTag("PlayerParent");
        for (int i = 0; i < players.Length; i++)
        {
            MyPlayersSorted.Add(players[i]);
        }
    }

    private void Update()
    {
        switch (currentState)
        {
            case GameState.none:
                break;
            case GameState.preGame:
                CountDown();
                break;
            case GameState.inGame:
                RankPlayers();
                break;
            case GameState.endGame:
                break;
            default:
                break;
        }
    }

    private void CountDown()
    {
        if (countDownTime > -2)
        {
            if (countDownTime <= 0)
            {
                countDownTimeText.text = "Go Noobs!";
            }
            else
            {
                countDownTimeText.text = countDownTime.ToString();
            }

            if (countDownTime <= 3 && countDownTimeText.fontSize < maxFontSize)
            {
                countDownTimeText.fontSize += fontSizeIncrementSpeed;
            }

            if (countDownTimer.IsReady())
            {
                countDownTimer.Reset();
                countDownTime -= timerCD;

                if(countDownTime >= 0)
                    countDownTimeText.fontSize = fontSize;
            }
        }
        else
        {
            countDownTimeText.gameObject.SetActive(false);
            currentState = GameState.inGame;
        }
    }

    private void RankPlayers()
    {
        MyPlayersSorted.Sort(SortByDistance);
    }

    private int SortByDistance(GameObject A, GameObject B)
    {
        return -(A.transform.position.z - finishLine.position.z).CompareTo(B.transform.position.z - finishLine.position.z);
    }

}
