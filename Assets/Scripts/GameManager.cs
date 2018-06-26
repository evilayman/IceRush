using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public Transform finishLine, deadZone;
    public GameObject deadPlayer;
    public TextMeshPro debugText;
    private bool endWithOne;

    private List<GameObject> myPlayersSorted, finishedPlayers;
    private List<float> finishTime;

    private int playersLoaded, playerList;
    private float waitTime;
    private bool inGameFirstTime, endGameFirstTime, stopCheck, startWait, offline = true;
    private AudioManager AM;

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
        AM = FindObjectOfType<AudioManager>();
        MyPlayersSorted = new List<GameObject>();
        finishedPlayers = new List<GameObject>();
        finishTime = new List<float>();
        playerList = PhotonNetwork.playerList.Length;

        if (playerList > 1)
            endWithOne = true;

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
            MyPlayersSorted.Add(players[i].transform.GetChild(0).gameObject);
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
                    GetComponent<ObjectsShowHide>().StartShow();
                    GameObject GM = new GameObject();
                    AM.Play("Theme", Instantiate(GM, transform));
                }
                CheckSurvivingPlayers();
                MyPlayersSorted.Sort(SortByDistance);
                CheckFinishLine();
                break;
            case GameState.goalReached:
                CheckSurvivingPlayers();
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

    private void CheckSurvivingPlayers()
    {
        if(endWithOne && MyPlayersSorted.Count <= 1)
        {
            finishedPlayers.Add(MyPlayersSorted[0]);
            finishTime.Add(Time.time);
            MyPlayersSorted.RemoveAt(0);
            currentState = GameState.endGame;
        }
    }

    private void CompleteFinishList()
    {
        for (int i = 0; i < MyPlayersSorted.Count; i++)
        {
            finishedPlayers.Add(MyPlayersSorted[i]);

            if (MyPlayersSorted[i].transform.parent.tag == "Player")
            {
                MyPlayersSorted[i].GetComponentInParent<PlayerManagerForNetwork>().currentPlayerState = PlayerManagerForNetwork.PlayerState.SlowToStop;
                finishTime.Add(0);
            }
            else
                finishTime.Add(-1);
        }
    }

    private void FinalScreen()
    {
        for (int i = 0; i < finishedPlayers.Count; i++)
        {
            if (finishedPlayers[i].transform.parent.tag == "Player")
                print(finishedPlayers[i].GetComponentInParent<PhotonView>().owner.NickName + " - Time: " + finishTime[i]);
            else
                print(finishedPlayers[i].GetComponentInParent<DeadTextManager>().playerName.text + " - Time: " + finishTime[i]);
        }
        StartCoroutine(ReturnToMainMenu());
    }

    IEnumerator ReturnToMainMenu()
    {
        yield return new WaitForSeconds(5);
        //LoadSceneMode sceneMode = LoadSceneMode.Single;
        //SceneManager.LoadSceneAsync("UI", sceneMode);
        PhotonNetwork.LeaveRoom();
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("UI");
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
        
        //GameObject enterYourNameMenu = GameObject.Find("EnterNameFirst");
        //enterYourNameMenu.SetActive(false);
        //GameObject MainMenu = GameObject.Find("MainMenu");
        //MainMenu.SetActive(true);


    }
    private void CheckFinishLine()
    {
        if (MyPlayersSorted.Count > 0 && (MyPlayersSorted[0].transform.position.z - finishLine.position.z) >= 0)
        {
            if (currentState != GameState.goalReached)
                currentState = GameState.goalReached;

            MyPlayersSorted[0].GetComponentInParent<PlayerTextManager>().ReachGoal = true;
            MyPlayersSorted[0].GetComponentInParent<PlayerManagerForNetwork>().currentPlayerState = PlayerManagerForNetwork.PlayerState.SlowToStop;

            finishedPlayers.Add(MyPlayersSorted[0]);
            finishTime.Add(Time.time);

            MyPlayersSorted.RemoveAt(0);
        }
    }

    public int GetRank(GameObject GO)
    {
        return (myPlayersSorted.Count > 0) ? MyPlayersSorted.FindIndex(x => x == GO) + finishedPlayers.Count : -1;
    }

    public void DeathSwap(GameObject GO)
    {
        var index = MyPlayersSorted.FindIndex(x => x == GO);
        var name = MyPlayersSorted[index].GetComponentInParent<PhotonView>().owner.NickName;
        var go = Instantiate(deadPlayer, MyPlayersSorted[index].transform.position, Quaternion.identity);
        MyPlayersSorted[index].GetComponentInParent<PlayerTextManager>().playerRank.SetText("");
        MyPlayersSorted[index] = go.transform.GetChild(0).gameObject;
    }

    public Transform GetTarget(GameObject GO)
    {
        int index = MyPlayersSorted.FindIndex(x => x == GO);
        return (index == 0) ? null : MyPlayersSorted[index + 1].transform;
    }

    private int SortByDistance(GameObject A, GameObject B)
    {
        return -(A.transform.position.z - finishLine.position.z).CompareTo(B.transform.position.z - finishLine.position.z);
    }

    //private void ShowHideRegions()
    //{
    //    //if (player.transform.position.z > 500)
    //    //{

    //    if (player.transform.position.z % RegionSize == 0)
    //    {
    //        region=GameObject.Find("Region" + currentRegion);
    //        region.gameObject.SetActive(false);
    //        region = GameObject.Find("Region" + currentRegion + 2);
    //        if (region)
    //        {
    //            GameObject.Find("Region" + currentRegion + 2).gameObject.SetActive(true);
    //        }           
    //        currentRegion++;
    //        compNum += RegionSize;
    //    }
    //    //}
    //}

   
}
