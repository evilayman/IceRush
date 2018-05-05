﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public Stats myStats;
    public GameObject leftHand, rightHand;

    private bool Died = false;

    private void Start()
    {
        FadeFromBlack(0.2f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!Died && collision.gameObject.tag == "Area")
        {
            Died = true;
            FadeToBlack(0.2f);
            StartCoroutine(resetScene(1f));
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "BoostRegion")
        {
            myStats.baseSpeed += myStats.boostSpeed;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "BoostRegion")
        {
            myStats.baseSpeed -= myStats.boostSpeed;
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
}
