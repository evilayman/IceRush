using UnityEngine;

public class CurrentRoomCanvas : MonoBehaviour
{
    //public void OnClickStartSync()
    //{
    //    PhotonNetwork.LoadLevel("Main");
            //if (!PhotonNetwork.isMasterClient)
            //return;
    //}

    public void OnClickStartDelayed()
    {
        if (!PhotonNetwork.isMasterClient)
            return;
        PhotonNetwork.room.IsOpen = false;
        PhotonNetwork.room.IsVisible = false;
        PhotonNetwork.LoadLevel("MainOld");

    }

}
