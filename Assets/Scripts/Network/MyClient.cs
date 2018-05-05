using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyClient : MonoBehaviour
{
    public string gameVersion = "0.1";
    public GameObject ConnectPanel;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        ConnectToMaster();
    }

    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings(gameVersion);
    }

    private void OnConnectedToMaster()
    {

        string playerName = "Player" + Random.Range(0, 1000);
        PhotonNetwork.automaticallySyncScene = true;
        PhotonNetwork.playerName = playerName;
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        //ConnectPanel.SetActive(true);
        print("joined lobby");
        if (!PhotonNetwork.inRoom)
        {
            MainCanvasManager.Instance.LobbyCanvas.transform.SetAsLastSibling();
        }
        
    }

    private void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from Server");
    }
}
