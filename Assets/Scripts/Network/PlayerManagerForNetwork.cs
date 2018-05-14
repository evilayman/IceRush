using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManagerForNetwork : MonoBehaviour
{
    PhotonView photonView;

    public Stats myStats;
    
    public GameObject leftHand, rightHand;

    private WaitForSeconds wait;

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
        wait = new WaitForSeconds(1f);
        photonView = GetComponent<PhotonView>();
        if (photonView.isMine)
        {
            FadeFromBlack(0.2f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (photonView.isMine)
        {
            if (!Died && collision.gameObject.tag == "Area")
            {
                Debug.Log("HIT");
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
}
