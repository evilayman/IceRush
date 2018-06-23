using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLayoutGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject chosenRoom;
    [SerializeField]
    private GameObject roomsList;
    [SerializeField]
    private GameObject _playerListingPrefab;
    private GameObject PlayerListingPrefab
    {
        get { return _playerListingPrefab; }
    }
    [SerializeField]
    private TextMeshProUGUI roomState;

    private List<PlayerListing> _playerListings = new List<PlayerListing>();
    private List<PlayerListing> PlayerListings
    {
        get { return _playerListings; }
    }

    //photon calls this function when the master player leaves and it's called on all players
    private void OnMasterClientSwitched(PhotonPlayer newMasterClient)
    {
        PhotonNetwork.LeaveRoom();
        chosenRoom.SetActive(false);
        roomsList.SetActive(true);

    }

    //this function is called by photon when you join a room
    private void OnJoinedRoom()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        //MainCanvasManager.Instance.CurrentRoomCanvas.transform.SetAsLastSibling();
        PhotonPlayer[] photonPlayers = PhotonNetwork.playerList;
        for (int i = 0; i < photonPlayers.Length; i++)
        {
            PlayerJoinedRoom(photonPlayers[i]);
        }
    }

    //this function is called by photon when a player joins the room
    private void OnPhotonPlayerConnected(PhotonPlayer photonPlayer)
    {
        PlayerJoinedRoom(photonPlayer);
    }
    //this function is called by photon when a player leaves a room
    private void OnPhotonPlayerDisconnected(PhotonPlayer photonPlayer)
    {
        PlayerLeftRoom(photonPlayer);
    }

    private void PlayerLeftRoom(PhotonPlayer photonPlayer)
    {
        int index = PlayerListings.FindIndex(x => x.PhotonPlayer == photonPlayer);
        if (index != -1)
        {
            Destroy(PlayerListings[index].gameObject);
            PlayerListings.RemoveAt(index);
        }

    }
    private void PlayerJoinedRoom(PhotonPlayer photonPlayer)
    {
        if (photonPlayer == null)
            return;
        //incase there's a delay in network and a player is added twice
        PlayerLeftRoom(photonPlayer);

        GameObject playerListingObj = Instantiate(PlayerListingPrefab);
        playerListingObj.transform.SetParent(transform, false);

        PlayerListing playerListing = playerListingObj.GetComponent<PlayerListing>();
        playerListing.ApplyPhotonPlayer(photonPlayer);

        PlayerListings.Add(playerListing);
    }

    public void OnClickRoomState()
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        PhotonNetwork.room.IsOpen = !PhotonNetwork.room.IsOpen;
        PhotonNetwork.room.IsVisible = PhotonNetwork.room.IsOpen;
        if (PhotonNetwork.room.IsOpen)
        {
            roomState.text = "Room State: Open";
        }
        else
        {
            roomState.text = "Room State: Closed";
        }
    }

    public void OnClickLeaveRoom()
    {
     
        PhotonNetwork.LeaveRoom();
    }

}
