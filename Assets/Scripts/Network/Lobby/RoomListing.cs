using UnityEngine;
using UnityEngine.UI;
using TMPro;
//this code works with old shitty ui
//public class RoomListing : MonoBehaviour
//{
//    private Text _roomNameText;
//    private Text RoomNameText
//    {
//        get { return _roomNameText; }
//    }
//    public string roomName { get; private set; }

//    public bool Updated { set; get; }
//    void Start()
//    {
//        GameObject lobbyCanvasObj = MainCanvasManager.Instance.LobbyCanvas.gameObject;
//        if (lobbyCanvasObj == null)
//        {
//            return;
//        }
//        LobbyCanvas lobbyCanvas = lobbyCanvasObj.GetComponent<LobbyCanvas>();
//        Button button = GetComponent<Button>();
//        button.onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text));
//    }

//    private void OnDestroy()
//    {
//        Button button = GetComponent<Button>();
//        button.onClick.RemoveAllListeners();
//    }
//    public void SetRoomNameText(string text)
//    {
//        roomName = text;
//        RoomNameText.text = roomName;
//    }
//}


public class RoomListing : MonoBehaviour
{
    [SerializeField]
    private Toggle powerUps;
    [SerializeField]
    private TextMeshProUGUI playersCount;
    [SerializeField]
    private TextMeshProUGUI _roomNameText;
    private TextMeshProUGUI RoomNameText
    {
        get { return _roomNameText; }
    }
    public string roomName { get; private set; }

    public bool Updated { set; get; }
    void Start()
    {
        
        GameObject roomListObj = GameObject.Find("RoomsListParent"); ;
        if (roomListObj == null)
        {
            return;
        }
        LobbyCanvas lobbyCanvas = roomListObj.GetComponent<LobbyCanvas>();
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() => lobbyCanvas.OnClickJoinRoom(RoomNameText.text));
    }

    private void OnDestroy()
    {
        Button button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }
    public void SetRoomNameAndPlayerCountText(string text)
    {
        roomName = text;
        RoomNameText.text = roomName;
        //print(PhotonNetwork.playerList.Length);
        playersCount.text = PhotonNetwork.playerList.Length.ToString();
    }
}
