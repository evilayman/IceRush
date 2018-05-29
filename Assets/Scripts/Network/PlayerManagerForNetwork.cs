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
    public GameObject model, headTarget, leftHand, rightHand, colliderObject;
    public PlayerState currentPlayerState;
    public float fadeTime, slowTime, respwanTime, pauseColTime, pauseSlowTime;

    private Vector3 spawnPoint;
    private PhotonView photonView;
    private GameManager GM;

    private bool inBoostRegion, inRespwan = false, inSlow = false, inGameFirstTime;
    private bool canCol = true, canSlow = true;
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
            if (!inRespwan && (collision.gameObject.tag == "Building" || collision.gameObject.tag == "Ground"))
            {
                StartCoroutine(Respwan());
            }
        }
    }

    [PunRPC]
    private void RPC_Collision()
    {
        if (canCol && !inRespwan)
            StartCoroutine(Respwan());
    }

    public void DroneHit(CreateDronePattern.MyDangerLevel danger)
    {
        switch (danger)
        {
            case CreateDronePattern.MyDangerLevel.Slow:
                if (canSlow && !inSlow)
                    StartCoroutine(Slow());
                break;
            case CreateDronePattern.MyDangerLevel.Respwan:
                if (canCol && !inRespwan)
                    StartCoroutine(Respwan());
                break;
            case CreateDronePattern.MyDangerLevel.Death:
                Debug.Log("Death");
                break;
            default:
                break;
        }
    }

    private IEnumerator Respwan()
    {
        inRespwan = true;
        FadeToBlack(fadeTime);
        currentPlayerState = PlayerState.Stopped;
        yield return new WaitForSeconds(respwanTime);
        inRespwan = false;
        transform.position = SpawnPoint;
        currentPlayerState = PlayerState.Normal;
        FadeFromBlack(fadeTime);

        canCol = false;
        canSlow = false;
        yield return new WaitForSeconds(pauseColTime);
        canCol = true;
        canSlow = true;
    }

    private IEnumerator Slow()
    {
        inSlow = true;
        currentPlayerState = PlayerState.Slowed;
        yield return new WaitForSeconds(slowTime);
        inSlow = false;
        currentPlayerState = PlayerState.Normal;

        canSlow = false;
        yield return new WaitForSeconds(pauseSlowTime);
        canSlow = true;
    }

    private IEnumerator StopCollider(float time)
    {
        colliderObject.SetActive(false);
        yield return new WaitForSeconds(time);
        colliderObject.SetActive(true);
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
