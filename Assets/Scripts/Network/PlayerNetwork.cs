using System.Collections;
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
        if (scene.name == "SortingScene")
        {
            if (PhotonNetwork.isMasterClient)
            {
                MasterLoadedGame();
                RPC_LoadedGameScene();
            }
            else
            {
                NonMasterLoadedGame();
            }
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
        PhotonNetwork.LoadLevel("SortingScene");
    }

    [PunRPC]
    private void RPC_LoadedGameScene()
    {
        playersInGame++;
        if (playersInGame == PhotonNetwork.playerList.Length)
        {
            photonView.RPC("RPC_CreatePlayer", PhotonTargets.All);
        }
    }

    [PunRPC]
    private void RPC_CreatePlayer()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().Offline = false;
        GameObject.Find("GameManager").GetComponent<GameManager>().currentState = GameManager.GameState.none;
        int spawnPosInx = 1;
        for (int i = 0; i < PhotonNetwork.playerList.Length; i++)
        {
            if (PhotonNetwork.playerList[i] == PhotonNetwork.player)
                spawnPosInx = (i - 1) * 2;

        }

        instantiatedPlayer = PhotonNetwork.Instantiate(Path.Combine("Prefabs", "PlayerSkateNetwork"), new Vector3(spawnPosInx, 50, 0), Quaternion.identity, 0);
    }
}