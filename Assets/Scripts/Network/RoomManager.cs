using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class RoomManager : MonoBehaviour
{
    
    [SerializeField]
    private Text playerName;
    [SerializeField]
    private Text _roomName;
    private Text RoomName
    {
        get { return _roomName; }
    }

    private void Awake()
    {
        // uncomment these two lines if the network is laggy or don't 
        //PhotonNetwork.sendRate = 60;
        //PhotonNetwork.sendRateOnSerialize = 30;
    }
    public void OnEndEditingPlayerName()
    {
        PhotonNetwork.playerName = playerName.text;
    }
    public void OnClick_CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 3 }; 
        if (PhotonNetwork.CreateRoom(RoomName.text,roomOptions,TypedLobby.Default))
        {
            //print("created room succseffuly");
               
        }
    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        //print("create room failed " + codeAndMessage[1]);
    }

}
