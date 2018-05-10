using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManagerForNetwork : MonoBehaviour
{
    PhotonView photonView;
    public Stats myStats;
    public GameObject leftHand, rightHand;

    private bool Died = false;

    private void Start()
    {
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
                Died = true;
                FadeToBlack(0.2f);
                StartCoroutine(ReturnTOLastRespawnPoint(1f));
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (photonView.isMine)
        {
            if (other.gameObject.name == "BoostRegion")
            {
                myStats.baseSpeed += myStats.boostSpeed;
            }
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (photonView.isMine)
        {
            if (other.gameObject.name == "BoostRegion")
            {
                myStats.baseSpeed -= myStats.boostSpeed;
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

    IEnumerator resetScene(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Main");
       
    }
    IEnumerator ReturnTOLastRespawnPoint(float time)
    {
       
        yield return new WaitForSeconds(time);
        this.transform.position = new Vector3(0, 50, 0);
    }
}
