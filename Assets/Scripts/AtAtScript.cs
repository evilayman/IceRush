using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AtAtScript : MonoBehaviour
{
    public float screamTime;
    public float screamDistance;
    AudioManager AM;
    CooldownTimer CDWalk, CDScream;
    GameObject bot, top;
    GameManager GM;

	void Start ()
    {
        GM = FindObjectOfType<GameManager>();
        AM = FindObjectOfType<AudioManager>();

        CDWalk = new CooldownTimer(0.5f, false);
        CDScream = new CooldownTimer(screamTime, true);

        bot = transform.GetChild(0).gameObject;
        top = transform.GetChild(1).gameObject;
    }

    void Update ()
    {
        if(CDWalk.IsReady())
        {
            CDWalk.Reset();
            AM.Play("WalkAtAt", bot);
        }

        var playerPos = GM.MyPlayersSorted[GM.MyPlayersSorted.Count - 1].transform.position;

        if (Vector3.Distance(playerPos, transform.position) < screamDistance)
        {
            if (CDScream.IsReady())
            {
                CDScream.Reset();
                AM.Play("ScreamATAT", top);
            }
        }


        
    }
}
