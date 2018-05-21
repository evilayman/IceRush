using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public enum GameState
    {
        none,
        preGame,
        inGame,
        endGame
    }

    public GameState currentState;

    private void Update()
    {
        switch (currentState)
        {
            case GameState.none:
                break;
            case GameState.preGame:
                break;
            case GameState.inGame:
                break;
            case GameState.endGame:
                break;
            default:
                break;
        }
    }
}
