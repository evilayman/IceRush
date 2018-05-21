using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManagerForNetwork : MonoBehaviour
{
    public Stats myStats;
    public GameObject leftHand, rightHand;
    public TextMeshPro playerName;

    private PhotonView photonView;

    private bool inBoostRegion, Died = false;

    public bool InBoostRegion
    {
        get
        {
            return inBoostRegion;
        }

        set
        {
            inBoostRegion = value;
        }
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.isMine)
        {
            FadeFromBlack(0.2f);
        }

        if (!photonView.isMine)
        {
            playerName.SetText(photonView.owner.NickName);
        }

    }

    private void Update()
    {
        if (!photonView.isMine)
        {
            rotateText();
        }
    }

    private void rotateText()
    {
        playerName.transform.rotation = Camera.main.transform.rotation;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.isMine)
        {
            if (!Died && collision.gameObject.tag == "Area")
            {
                Died = true;
                FadeToBlack(0.2f);
                StartCoroutine(ReturnToLastRespawnPoint());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.isMine)
        {
            if (other.gameObject.name == "BoostRegion")
            {
                InBoostRegion = true;

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.isMine)
        {
            if (other.gameObject.name == "BoostRegion")
            {
                InBoostRegion = false;
            }
        }
    }

    private void FadeToBlack(float time)
    {
        //set start color
        SteamVR_Fade.Start(Color.clear, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.black, time);
    }

    private void FadeFromBlack(float time)
    {
        //set start color
        SteamVR_Fade.Start(Color.black, 0f);
        //set and start fade to
        SteamVR_Fade.Start(Color.clear, time);
    }

    IEnumerator ReturnToLastRespawnPoint()
    {
        yield return new WaitForSeconds(1f);
        this.transform.position = new Vector3(0, 50, 0);
        Died = false;
    }

}
