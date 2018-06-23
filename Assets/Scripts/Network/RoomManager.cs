using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI maxPlayers;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI _roomName;
    private TextMeshProUGUI RoomName
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
        print(PhotonNetwork.playerName);
    }
    public void OnClick_CreateRoom()
    {
        int maxPlayersInRoom = int.Parse(maxPlayers.text);
        if (maxPlayersInRoom > 3)
        {
            maxPlayersInRoom = 3;
        }
        if (maxPlayersInRoom < 2)
        {
            maxPlayersInRoom = 2;
        }

        RoomOptions roomOptions = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = (byte)maxPlayersInRoom };
        if (PhotonNetwork.CreateRoom(RoomName.text, roomOptions, TypedLobby.Default))
        {
            print("created room succseffuly");

        }
       

    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        //print("create room failed " + codeAndMessage[1]);
    }

}
