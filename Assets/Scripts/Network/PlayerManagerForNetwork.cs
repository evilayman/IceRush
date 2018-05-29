using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManagerForNetwork : MonoBehaviour
{
    public enum PlayerState
    {
        Stopped,
        Normal,
        Slowed,
        SlowToStop
    }

    public Stats myStats;
    public GameObject model, headTarget, leftHand, rightHand;
    public PlayerState currentPlayerState;
    public float fadeTime, respwanTime;

    private Vector3 spawnPoint;
    private PhotonView photonView;
    private GameManager GM;

    private bool inBoostRegion, Hit = false, inGameFirstTime;

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
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (photonView.isMine)
        {
            model.gameObject.SetActive(false);
            GM.gameObject.GetPhotonView().RPC("RPC_playerLoaded", PhotonTargets.MasterClient);
        }
        else
        {
            leftHand.transform.GetChild(0).gameObject.SetActive(false);
            rightHand.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (photonView.isMine || GM.Offline)
        {
            if (!inGameFirstTime && GM.currentState == GameManager.GameState.inGame)
            {
                inGameFirstTime = true;
                currentPlayerState = PlayerState.Normal;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.isMine || GM.Offline)
        {
            Debug.Log(collision.gameObject.name);
            if (!Hit && (collision.gameObject.tag == "Building" || collision.gameObject.tag == "Ground"))
            {
                Respwan(respwanTime);
            }
        }
    }

    [PunRPC]
    private void RPC_Collision()
    {
        Respwan(respwanTime);
    }

    public void Respwan(float time)
    {
        Hit = true;
        currentPlayerState = PlayerState.Stopped;
        FadeToBlack(fadeTime);
        StartCoroutine(ReturnToLastRespawnPoint(time));
    }

    private IEnumerator ReturnToLastRespawnPoint(float time)
    {
        yield return new WaitForSeconds(time);
        FadeFromBlack(fadeTime);
        Hit = false;
        transform.position = SpawnPoint;
        currentPlayerState = PlayerState.Normal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.isMine || GM.Offline)
        {
            if (other.gameObject.tag == "BoostRegion")
            {
                InBoostRegion = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.isMine || GM.Offline)
        {
            if (other.gameObject.tag == "BoostRegion")
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

}
