using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPowerUp : MonoBehaviour
{
    public float resetTime;
    private PhotonView photonView;
    private WaitForSeconds PowerUpTimer;
    private GameManager GM;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        PowerUpTimer = new WaitForSeconds(resetTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerCollider")
        {
            SetPowerUp(other.gameObject);
            photonView.RPC("RPC_ResetPowerUp", PhotonTargets.All);
        }
    }

    private void SetPowerUp(GameObject player)
    {
        var i = GM.GetRank(player.transform.GetChild(0).gameObject);
        player.gameObject.GetComponentInParent<UsePowerUp>().CurrentPower = CalculatePowerUp(i);
    }

    private UsePowerUp.PowerUpType CalculatePowerUp(int index)
    {
        

        return UsePowerUp.PowerUpType.Rocket;
    }

    [PunRPC]
    private void RPC_ResetPowerUp()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(ResetPowerUp());
    }

    IEnumerator ResetPowerUp()
    {
        yield return PowerUpTimer;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}