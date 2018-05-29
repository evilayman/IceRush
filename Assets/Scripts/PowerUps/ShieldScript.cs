using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldScript : Photon.MonoBehaviour
{
    [SerializeField]
    private float shieldDuration;
    private WaitForSeconds shieldTime;
    private bool justTurnedOn;
    [SerializeField]
    private GameObject rocketChild;
    private PhotonView photonView;

    public bool JustTurnedOn
    {
        get
        {
            return justTurnedOn;
        }

        set
        {
            justTurnedOn = value;
        }
    }

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        shieldTime = new WaitForSeconds(shieldDuration);
    }

    private void Update()
    {
        if (!justTurnedOn && rocketChild.activeInHierarchy)
        {
            justTurnedOn = true;
            photonView.RPC("RPC_EnableDisableShieldToOtherPlayers", PhotonTargets.Others, photonView.viewID, justTurnedOn);
            StartCoroutine(TurnOffShield());
        }
    }

    IEnumerator TurnOffShield()
    {
        yield return shieldTime;
        justTurnedOn = false;
        rocketChild.SetActive(false);
        photonView.RPC("RPC_EnableDisableShieldToOtherPlayers", PhotonTargets.Others, photonView.viewID, justTurnedOn);
    }

    [PunRPC]
    void RPC_EnableDisableShieldToOtherPlayers(int shieldParentViewID, bool setActive)
    {
        PhotonView view = PhotonView.Find(shieldParentViewID);
        view.gameObject.transform.Find("ShieldInnerChild").gameObject.SetActive(setActive);
    }

   
}



