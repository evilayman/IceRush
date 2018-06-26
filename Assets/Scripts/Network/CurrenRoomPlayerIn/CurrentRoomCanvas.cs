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
        FadeToBlack(0.2f);
        PhotonNetwork.LoadLevel("SortingScene");


    }

    private void FadeToBlack(float time)
    {
        //set start color
        SteamVR_Fade.Start(Color.clear, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.black, time);
    }


}