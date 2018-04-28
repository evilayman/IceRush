using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RoomManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public InputField RoomName;

    private void Awake()
    {
        DontDestroyOnLoad(transform);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;

        // uncomment these two lines if the network is laggy or don't 
        //PhotonNetwork.sendRate = 60;
        //PhotonNetwork.sendRateOnSerialize = 30;
    }

    public void OnClickCreateOrJoin()
    {
        if (RoomName.text.Length >= 1)
            PhotonNetwork.JoinOrCreateRoom(RoomName.text, new RoomOptions() { MaxPlayers = 3 }, null);
    }

    private void OnCreatedRoom()
    {
    }

    private void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameRoom");
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameRoom")
        {
            SpawnPlayer();
        }
    }

    private void SpawnPlayer()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, playerPrefab.transform.rotation, 0);
    }
}
