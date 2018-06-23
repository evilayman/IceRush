﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            print("joined room");
        }
        else
	    {
            print("Join Room Failed");
        }
        
    }
}
