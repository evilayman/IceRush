using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using TMPro;

public class PlayerManagerForNetwork : MonoBehaviour
{
    public Stats myStats;
    public GameObject leftHand, rightHand;

    private Vector3 spawnPoint;
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
    public Vector3 SpawnPoint
    {
        get
        {
            return spawnPoint;
        }

        set
        {
            spawnPoint = value;
        }
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.isMine)
        {
            FadeFromBlack(0.5f);
        }
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

    IEnumerator ReturnToLastRespawnPoint()
    {
        yield return new WaitForSeconds(1f);
        transform.position = SpawnPoint;
        Died = false;
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

}
