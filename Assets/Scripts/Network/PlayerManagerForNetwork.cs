using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerManagerForNetwork : MonoBehaviour
{
    private GameManager GM;
    public Stats myStats;
    public GameObject leftHand, rightHand;
    public TextMeshPro playerName, playerRank;
    internal Transform spawnPoint;
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
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (photonView.isMine)
        {
            FadeFromBlack(0.5f);
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
            setRankText();
            rotateText();
        }
    }

    private void rotateText()
    {
        if(Camera.main)
            playerName.transform.rotation = playerRank.transform.rotation = Camera.main.transform.rotation;
    }

    private void setRankText()
    {
        var i = GM.MyPlayersSorted.FindIndex(x => x == gameObject);
        playerRank.SetText(GetRankString(i));
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
        this.transform.position = spawnPoint.position;
        Died = false;
    }

    private string GetRankString(int index)
    {
        string rank;
        switch (index)
        {
            case 0:
                rank = "1st";
                break;
            case 1:
                rank = "2nd";
                break;
            case 2:
                rank = "3rd";
                break;
            default:
                rank = "";
                break;
        }
        return rank;
    }

}
