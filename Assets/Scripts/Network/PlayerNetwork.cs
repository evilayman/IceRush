﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class PlayerNetwork : MonoBehaviour
{   
    public static PlayerNetwork instance;
    public string playerName { get; private set; }
    private PhotonView photonView;
    private int playersInGame;
    private GameObject instantiatedPlayer;
    private void Awake()
    {
        instance = this;
        photonView = GetComponent<PhotonView>();
        //playerName = "Player" + Random.Range(0, 1000);
        SceneManager.sceneLoaded += OnSceneFinishedLoading;
    }

    private void OnSceneFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main")
        {
            if (PhotonNetwork.isMasterClient)
            {
                MasterLoadedGame();
                NonMasterLoadedGame();

            }
            else
                NonMasterLoadedGame();

        }
    }

    private void MasterLoadedGame()
    {
        //playersInGame = 1;
        photonView.RPC("RPC_LoadGameForOthers", PhotonTargets.Others);
    }

    private void NonMasterLoadedGame()
    {
        photonView.RPC("RPC_LoadedGameScene", PhotonTargets.MasterClient);
    }

    [PunRPC]
    private void RPC_LoadGameForOthers()
    {
        PhotonNetwork.LoadLevel("Main");
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.playerList.Length)
        {
            //print("All players are in game");
            photonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        float randomValue = Random.Range(0f, 50f);
        instantiatedPlayer = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerSkateNetwork"), new Vector3(randomValue, 50, 0), Quaternion.identity, 0);
        instantiatedPlayer.GetComponentInChildren<Text>().text=PhotonNetwork.playerName;

    }

}
