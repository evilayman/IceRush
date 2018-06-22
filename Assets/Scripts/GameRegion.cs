using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRegion : MonoBehaviour
{
    public float speed, regionSize, stopPoint;
    private GameManager GM;
    public GameObject AtAt, BackBorder;

    private void Start()
    {
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (GM.currentState == GameManager.GameState.inGame)
        {
            if (transform.position.z < stopPoint)
                transform.position += Vector3.forward * speed * Time.deltaTime;
            else if (AtAt.transform.position.z < stopPoint + 350)
            {
                AtAt.transform.position += Vector3.forward * speed * Time.deltaTime;
                BackBorder.transform.position += Vector3.forward * speed * Time.deltaTime;
            }
        }
    }
}
