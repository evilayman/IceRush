using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class RoomManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enterYourNameMenu;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject createRoomMenu;
    [SerializeField]
    private GameObject chosenRoomMenu;
    [SerializeField]
    private TextMeshProUGUI maxPlayers;
    [SerializeField]
    private TextMeshProUGUI playerName;
    [SerializeField]
    private TextMeshProUGUI _roomName;
    [HideInInspector]
    public TextMeshProUGUI RoomName
    {
        get { return _roomName; }
    }

    private void Start()
    {
        if(PhotonNetwork.playerName != "")
        {
            enterYourNameMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
    private void Awake()
    {
        // uncomment these two lines if the network is laggy or don't 
        //PhotonNetwork.sendRate = 60;
        //PhotonNetwork.sendRateOnSerialize = 30;
    }
    public void OnEndEditingPlayerName()
    {
        if (playerName.text.Length >= 3)
        {
            PhotonNetwork.playerName = playerName.text;
            enterYourNameMenu.SetActive(false);
            mainMenu.SetActive(true);
        }

    }
    public void OnClick_CreateRoom()
    {
        int maxPlayersInRoom;

        if (_roomName.text.Length >= 3 && maxPlayers.text.Length == 2)
        {
            if (int.TryParse(maxPlayers.text, out maxPlayersInRoom))
            {
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
                    
                    
                    createRoomMenu.SetActive(false);
                    chosenRoomMenu.SetActive(true);

                }
            }
        }



    }

    private void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        //print("create room failed " + codeAndMessage[1]);
    }
    
    public void OnClickOnARoomToJoin(GameObject myRoomName)
    {
        _roomName = GetComponent<TextMeshProUGUI>();
    }

}
