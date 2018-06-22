using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this code works with old shitty UI
//public class RoomLayoutGroup : MonoBehaviour
//{
//    [SerializeField]
//    private GameObject _roomListingPrefab;
//    private GameObject RoomListingPrefab
//    {
//        get { return _roomListingPrefab; }
//    }
//    private List<RoomListing> _roomListingButtons = new List<RoomListing>();
//    private List<RoomListing> RoomListingButtons
//    {
//        get { return _roomListingButtons; }
//    }
//    private void OnReceivedRoomListUpdate()
//    {
//        RoomInfo[] rooms = PhotonNetwork.GetRoomList();

//        foreach (RoomInfo room in rooms)
//        {
//            RoomReceived(room);
//        }

//        RemoveOldRooms();
//    }

//    private void RoomReceived(RoomInfo room)
//    {
//        int index = RoomListingButtons.FindIndex(x => x.roomName == room.Name);
//        if (index == -1)
//        {
//            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
//            {
//                GameObject roomListingObj = Instantiate(RoomListingPrefab);
//                roomListingObj.transform.SetParent(transform, false);

//                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
//                RoomListingButtons.Add(roomListing);

//                index = (RoomListingButtons.Count - 1);
//            }
//        }

//        if (index != -1)
//        {
//            RoomListing roomListing = RoomListingButtons[index];
//            roomListing.SetRoomNameText(room.Name);
//            roomListing.Updated = true;
//        }
//    }

//    private void RemoveOldRooms()
//    {
//        List<RoomListing> removeRooms = new List<RoomListing>();
//        foreach (RoomListing roomListing in RoomListingButtons)
//        {
//            if (!roomListing.Updated)
//            {
//                removeRooms.Add(roomListing);
//            }
//            else
//            {
//                roomListing.Updated = false;
//            }
//        }
//        foreach (RoomListing roomListing in removeRooms)
//        {
//            GameObject roomListingObj = roomListing.gameObject;
//            RoomListingButtons.Remove(roomListing);
//            Destroy(roomListingObj);
//        }
//    }
//}

public class RoomLayoutGroup : MonoBehaviour
{
    [SerializeField]
    private GameObject _roomListingPrefab;
    private GameObject RoomListingPrefab
    {
        get { return _roomListingPrefab; }
    }
    private List<RoomListing> _roomListingButtons = new List<RoomListing>();
    private List<RoomListing> RoomListingButtons
    {
        get { return _roomListingButtons; }
    }
    private void Update()
    {
  
        RoomInfo[] rooms = PhotonNetwork.GetRoomList();
        //print(rooms.Length);

        foreach (RoomInfo room in rooms)
        {
            RoomReceived(room);

        }
        RemoveOldRooms();
    }
    //private void OnReceivedRoomListUpdate()
    //{
    //    print("in");
    //    RoomInfo[] rooms = PhotonNetwork.GetRoomList();
    //    print(rooms.Length);

    //    foreach (RoomInfo room in rooms)
    //    {
    //        RoomReceived(room);
            
    //    }
    //    RemoveOldRooms();
    //}

    private void RoomReceived(RoomInfo room)
    {
        int index = RoomListingButtons.FindIndex(x => x.roomName == room.Name);
        if (index == -1)
        {
            if (room.IsVisible && room.PlayerCount < room.MaxPlayers)
            {
                GameObject roomListingObj = Instantiate(RoomListingPrefab);
                roomListingObj.transform.SetParent(transform, false);
                roomListingObj.SetActive(true);

                RoomListing roomListing = roomListingObj.GetComponent<RoomListing>();
                RoomListingButtons.Add(roomListing);

                index = (RoomListingButtons.Count - 1);
            }
        }

        if (index != -1)
        {
            RoomListing roomListing = RoomListingButtons[index];
            roomListing.SetRoomNameAndPlayerCountText(room.Name);
            roomListing.Updated = true;
        }
    }

    private void RemoveOldRooms()
    {
        List<RoomListing> removeRooms = new List<RoomListing>();
        foreach (RoomListing roomListing in RoomListingButtons)
        {
            if (!roomListing.Updated)
            {
                removeRooms.Add(roomListing);
            }
            else
            {
                roomListing.Updated = false;
            }
        }
        foreach (RoomListing roomListing in removeRooms)
        {
            GameObject roomListingObj = roomListing.gameObject;
            RoomListingButtons.Remove(roomListing);
            Destroy(roomListingObj);
        }
    }
}