using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPowerUp : MonoBehaviour
{
    public float resetTime;

    [Range(0, 5)]
    [Space(5)]
    public float rocketPriorityFront;
    [Range(0, 5)]
    public float rocketPriorityEnd;
    [Range(0, 5)]
    [Space(5)]
    public float shieldPriorityFront;
    [Range(0, 5)]
    public float shieldPriorityEnd;
    [Range(0, 5)]
    [Space(5)]
    public float teleportPriorityFront;
    [Range(0, 5)]
    public float teleportPriorityEnd;
    [Range(0, 5)]
    [Space(5)]
    public float boostPriorityFront;
    [Range(0, 5)]
    public float boostPriorityEnd;
    [Range(0, 5)]
    [Space(5)]
    public float trapPriorityFront;
    [Range(0, 5)]
    public float trapPriorityEnd;

    private PhotonView photonView;
    private WaitForSeconds PowerUpTimer;
    private GameManager GM;
    private float rank, numPlayers;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
        PowerUpTimer = new WaitForSeconds(resetTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerCollider")
        {
            numPlayers = GM.MyPlayersSorted.Count;
            rank = GM.MyPlayersSorted.FindIndex(x => x == other.transform.parent.GetChild(0).gameObject) + 1;

            if (numPlayers == 1)
                numPlayers++;

            other.gameObject.GetComponentInParent<UsePowerUp>().CurrentPower = CalculatePowerUp();

            photonView.RPC("RPC_ResetPowerUp", PhotonTargets.All);
        }
    }

    private UsePowerUp.PowerUpType CalculatePowerUp()
    {
        //List<float> percList = GetPercentages();

        //for (int i = 1; i < percList.Count; i++)
        //{
        //    percList[i] += percList[i - 1];
        //    if (percList[i] == 100)
        //        break;
        //}

        //var randomValue = Random.Range(0, 100f);

        //if (randomValue <= percList[0])
        //    return UsePowerUp.PowerUpType.Rocket;
        //else if (randomValue > percList[0] && randomValue <= percList[1])
        //    return UsePowerUp.PowerUpType.Sheild;
        //else if (randomValue > percList[1] && randomValue <= percList[2])
        //    return UsePowerUp.PowerUpType.Teleport;
        //else if (randomValue > percList[2] && randomValue <= percList[3])
        //    return UsePowerUp.PowerUpType.Boost;
        //else if (randomValue > percList[3] && randomValue <= percList[4])
        //    return UsePowerUp.PowerUpType.Trap;
        //else
        //    return UsePowerUp.PowerUpType.None;
        print("got power up");
        return UsePowerUp.PowerUpType.Teleport;

    }

    private List<float> GetPercentages()
    {
        List<float> priorityList = new List<float>();

        priorityList.Add(GetPriority(rocketPriorityFront, rocketPriorityEnd));
        priorityList.Add(GetPriority(shieldPriorityFront, shieldPriorityEnd));
        priorityList.Add(GetPriority(teleportPriorityFront, teleportPriorityEnd));
        priorityList.Add(GetPriority(boostPriorityFront, boostPriorityEnd));
        priorityList.Add(GetPriority(trapPriorityFront, trapPriorityEnd));

        var sum = 0f;
        for (int i = 0; i < priorityList.Count; i++)
        {
            sum += priorityList[i];
        }
        var eachPrioPerc = 100 / sum;

        List<float> percList = new List<float>();
        for (int i = 0; i < priorityList.Count; i++)
        {
            percList.Add(priorityList[i] * eachPrioPerc);
            //Debug.Log(percList[i] + "%");
        }
        return percList;
    }

    private float GetPriority(float frontPriority, float endPriority)
    {
        var ratio = (endPriority - frontPriority) / (numPlayers - 1);
        var priority = (rank - 1) * ratio + frontPriority;
        return priority;
    }

    [PunRPC]
    private void RPC_ResetPowerUp()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        StartCoroutine(ResetPowerUp());
    }

    IEnumerator ResetPowerUp()
    {
        yield return PowerUpTimer;
        transform.GetChild(0).gameObject.SetActive(true);
    }
}