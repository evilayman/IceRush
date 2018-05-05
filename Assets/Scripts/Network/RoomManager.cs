using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class RoomManager : MonoBehaviour
{
    //public GameObject playerPrefab;
    

    [SerializeField]
    private Text _roomName;
    private Text RoomName
    {
        get { return _roomName; }
    }

    private void Awake()
    {
        
        //SceneManager.sceneLoaded += OnSceneFinishedLoading;

        // uncomment these two lines if the network is laggy or don't 
        //PhotonNetwork.sendRate = 60;
        //PhotonNetwork.sendRateOnSerialize = 30;
    }

    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 3 }; 
        if (PhotonNetwork.CreateRoom(RoomName.text,roomOptions,TypedLobby.Default))
        {
            print("created room succseffuly");
        }
        else
        {

        }
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        print("create room failed " + codeAndMessage[1]);
    }
    //public void OnClickCreateOrJoin()
    //{
    //    if (RoomName.text.Length >= 1)
    //        PhotonNetwork.JoinOrCreateRoom(RoomName.text, new RoomOptions() { MaxPlayers = 3 }, null);
    //}

    private void OnCreatedRoom()
    {
    }

    private void OnJoinedRoom()
    {
        //PhotonNetwork.LoadLevel("GameRoom");
    }

    //private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    //{
    //    if (scene.name == "GameRoom")
    //    {
    //        SpawnPlayer();
    //    }
    //}

    //private void SpawnPlayer()
    //{
    //    PhotonNetwork.Instantiate(playerPrefab.name, playerPrefab.transform.position, playerPrefab.transform.rotation, 0);
    //}
}
