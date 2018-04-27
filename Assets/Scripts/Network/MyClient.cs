using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyClient : MonoBehaviour
{
    public string gameVersion = "0.1";
    public GameObject ConnectPanel;

    private void Awake()
    {
        ConnectToMaster();
    }

    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings(gameVersion);
    }

    private void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    private void OnJoinedLobby()
    {
        ConnectPanel.SetActive(true);
    }

    private void OnDisconnectedFromPhoton()
    {
        Debug.Log("Disconnected from Server");
    }
}
