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

    public Transform spawnPoint;
    private PhotonView photonView;
    private GameManager GM;

    private bool inBoostRegion, inRespwan = false, inSlow = false, inGameFirstTime,
        canCol = true, canSlow = true, isDead;
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

    public bool IsDead
    {
        get
        {
            return isDead;
        }

        set
        {
            isDead = value;
        }
    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (photonView.isMine || GM.Offline)
        {
            //model.gameObject.SetActive(false);
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
            if (!inRespwan && (collision.gameObject.tag == "Building" || collision.gameObject.tag == "Ground"))
            {
                StartCoroutine(Respwan());
            }
            else if(collision.gameObject.tag == "ATAT")
            {
                photonView.RPC("RPC_Death", PhotonTargets.All);
            }
        }
    }

    [PunRPC]
    private void RPC_Collision()
    {
        if (canCol && !inRespwan)
            StartCoroutine(Respwan());
    }

    [PunRPC]
    private void RPC_Death()
    {
        if (!isDead)
            StartCoroutine(Death());
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
                //if (!isDead)
                //StartCoroutine(Death());
                photonView.RPC("RPC_Death", PhotonTargets.All);
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
        transform.position = spawnPoint.position;
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

    private IEnumerator Death()
    {
        FadeToBlack(fadeTime);
        IsDead = true;
        currentPlayerState = PlayerState.Stopped;
        GM.DeathSwap(transform.GetChild(0).gameObject);
        transform.position = GM.deadZone.position;
        yield return new WaitForSeconds(respwanTime);
        FadeFromBlack(fadeTime);
        currentPlayerState = PlayerState.Normal;
    }

    public IEnumerator BoostPlayer(object[] parms)
    {
        float boostTime = (float) parms[0];
        inBoostRegion = true;
        yield return new WaitForSeconds(boostTime);
        InBoostRegion = false;
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
