using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMovment : MonoBehaviour
{
    public float moveTowardsSpeed;
    public float maxRaduisDelta;
    public bool upOrDown;
    public List<Transform> posList;
    public List<Transform> PosList
    {
        get
        {
            return posList;
        }

        set
        {
            posList = value;
        }
    }

    private bool collided = false;
    private Transform player;
    private Vector3 targetDir;
    private Vector3 newDir;
    static int count = 0;
    private int index = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player = other.GetComponent<Transform>();
            collided = true;
        }
    }


    private void Update()
    {

        if (collided)
        {
            if (upOrDown)
            {
                player.position = Vector3.MoveTowards(player.position, posList[index].position, moveTowardsSpeed * Time.deltaTime);

            }
            else
            {
                Move(index);
            }
            if (player.position.z == posList[index].position.z)
            {
                if (index != posList.Count-1)
                {
                    index++;
                }
            }
        }

    }
    public void Move(int i)
    {
        targetDir = posList[i].position - player.position;
        float step = maxRaduisDelta * Time.deltaTime;
        newDir = Vector3.RotateTowards(player.forward, targetDir, step, 0.0f);
        player.rotation = Quaternion.LookRotation(newDir);

        player.position = Vector3.MoveTowards(player.position, posList[i].position, moveTowardsSpeed * Time.deltaTime);

    }
    public void AddPoint()
    {
        var go = new GameObject("" + count);
        go.transform.SetParent(transform);
        go.transform.position = new Vector3(0, 50, 0);
        go.transform.localScale = new Vector3(0, 0, 0);
        PosList.Add(go.transform);
        count++;
    }
}
