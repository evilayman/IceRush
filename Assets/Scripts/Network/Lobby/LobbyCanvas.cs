using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCanvas : MonoBehaviour
{
    [SerializeField]
    private RoomLayoutGroup _roomLayoutGroup;
    private RoomLayoutGroup roomLayoutGroup
    {
        get { return _roomLayoutGroup; }
    }
    public void OnClickJoinRoom(string roomName)
    {
        if (PhotonNetwork.JoinRoom(roomName))
        {
            
        }
        else
	    {
            print("Join Room Failed");
        }
        
    }
}
