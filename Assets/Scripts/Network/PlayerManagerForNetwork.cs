using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//using TMPro;

public class PlayerManagerForNetwork : MonoBehaviour
{
    public enum PlayerState
    {
        Stopped,
        Normal,
        Slowed, //Slow Player for time
        Respwaned, //When Player Hit's Object and Respwans (Disable Collider)
        SlowToStop
    }

    public Stats myStats;
    public GameObject leftHand, rightHand;
    public PlayerState currentPlayerState;

    private Vector3 spawnPoint;
    private PhotonView photonView;
    private GameManager GM;

    private bool inBoostRegion, Died = false, inGameFirstTime;

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
            FadeFromBlack(0.5f);
        }
    }

    private void Update()
    {
        if(photonView.isMine)
        {
            if (!inGameFirstTime && GM.currentState == GameManager.GameState.inGame)
            {
                inGameFirstTime = true;
                currentPlayerState = PlayerState.Normal;
            }

            if(currentPlayerState != PlayerState.Stopped && GetComponent<PlayerTextManager>().ReachGoal)
                currentPlayerState = PlayerState.Stopped;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.isMine)
        {
            if (!Died && (collision.gameObject.tag == "Building" || collision.gameObject.tag == "Ground"))
            {
                Collided();
            }
        }
    }

    [PunRPC]
    private void RPC_Collision()
    {
        Collided();
    }

    public void Collided()
    {
        Died = true;
        currentPlayerState = PlayerState.Stopped;
        FadeToBlack(0.2f);
        StartCoroutine(ReturnToLastRespawnPoint());
    }

    private IEnumerator ReturnToLastRespawnPoint()
    {
        yield return new WaitForSeconds(0.2f);
        Died = false;
        transform.position = SpawnPoint;
        currentPlayerState = PlayerState.Normal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.isMine)
        {
            if (other.gameObject.tag == "BoostRegion")
            {
                InBoostRegion = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.isMine)
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
