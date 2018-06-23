using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerListing  : MonoBehaviour
{
    public PhotonPlayer PhotonPlayer { get; private set; }
    [SerializeField]
    private TextMeshProUGUI playerName;
    private TextMeshProUGUI PlayerName
    {
        get { return playerName; }
    }
    public void ApplyPhotonPlayer(PhotonPlayer photonPlayer)
    {
        PhotonPlayer = photonPlayer;
        playerName.text = photonPlayer.NickName;
    }
	
}
