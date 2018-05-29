using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPowerUp : MonoBehaviour
{
    public float resetTime;
    private PhotonView photonView;
    private WaitForSeconds PowerUpTimer;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        PowerUpTimer = new WaitForSeconds(resetTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerCollider")
        {
            CalculatePowerUpType(other.gameObject);
            photonView.RPC("RPC_ResetPowerUp", PhotonTargets.All);
        }
    }

    private void CalculatePowerUpType(GameObject player)
    {
        int power = Random.Range(0, 2);
        print(power);
        if (power == 0)
        {
            print("got Rocket");
            player.gameObject.GetComponentInParent<UsePowerUp>().CurrentPower = UsePowerUp.PowerUpType.Rocket;
        }
        else
        {
            player.gameObject.GetComponentInParent<UsePowerUp>().CurrentPower = UsePowerUp.PowerUpType.Sheild;
            print("got Shield");
        }
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