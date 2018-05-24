using UnityEngine;
using UnityEngine.UI;

public class PlayerListing  : MonoBehaviour
{
    public PhotonPlayer PhotonPlayer { get; private set; }
    [SerializeField]
    private Text playerName;
    private Text PlayerName
    {
        get { return playerName; }
    }
    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        playerName.text = photonPlayer.NickName;
    }
	
}
