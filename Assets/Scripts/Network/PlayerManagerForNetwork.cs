using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerManagerForNetwork : MonoBehaviour
{
    private PhotonView photonView;
    private GameObject[] players;
    [SerializeField]
    private Text playerName;
    private WaitForSeconds wait, waitForPlayersList;
    [SerializeField]
    private float rotationSpeed;
    private Vector3 newDir;
    private bool inBoostRegion, Died = false;

    public Stats myStats;
    public GameObject leftHand, rightHand, thisClientHead;
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
        wait = new WaitForSeconds(1f);
        waitForPlayersList = new WaitForSeconds(3f);
        photonView = GetComponent<PhotonView>();

        if (photonView.isMine)
        {
            FadeFromBlack(0.2f);
            StartCoroutine(MakePlayersList());
        }

        if (!photonView.isMine)
        {
            playerName.text = photonView.owner.NickName;
        }

    }

    private void Update()
    {
        if (photonView.isMine)
            RotateOtherPlayersNames();
    }

    private void RotateOtherPlayersNames()
    {
        if (players!=null)
        {
            for (int i = 0; i < players.Length; i++)
            {
                //players[i].GetComponent<PlayerManagerForNetwork>().playerName.color = Color.red;
                var current = players[i].GetComponent<PlayerManagerForNetwork>().playerName.rectTransform.forward;
                var target = (players[i].transform.position - transform.position).normalized;
                newDir = Vector3.RotateTowards(current, target, rotationSpeed, 0.0f);
                players[i].GetComponentInChildren<Text>().rectTransform.rotation = Quaternion.LookRotation(newDir);
            }
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
                StartCoroutine(ReturnTOLastRespawnPoint());
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

    IEnumerator ReturnTOLastRespawnPoint()
    {
        yield return wait;
        this.transform.position = new Vector3(0, 50, 0);
        Died = false;
    }
    IEnumerator MakePlayersList()
    {
        yield return waitForPlayersList;
        players = GameObject.FindGameObjectsWithTag("PlayerParent");
    }
}
