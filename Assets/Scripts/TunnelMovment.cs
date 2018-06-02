using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelMovment : MonoBehaviour
{
    public float moveTowardsSpeed;
    public float rotateSpeed;
    public bool upOrDown;
    public bool drawPath;

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

    private void Start()
    {
        posList = new List<Transform>();
        AddChildren();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollider" && !collided)
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
                if (index != posList.Count - 1)
                    index++;
            }
        }

    }

    public void Move(int i)
    {
        targetDir = posList[i].position - player.position;
        float step = rotateSpeed * Time.deltaTime;
        newDir = Vector3.RotateTowards(player.forward, targetDir, step, 0.0f);
        player.rotation = Quaternion.LookRotation(newDir);

        player.parent.transform.position = Vector3.MoveTowards(player.parent.transform.position, posList[i].position,
            moveTowardsSpeed * Time.deltaTime);
    }

    public void AddPoint()
    {
        var go = new GameObject("" + count);
        go.transform.SetParent(transform);
        go.transform.position = transform.position;
        PosList.Add(go.transform);
        count++;
    }
    public void AddChildren()
    {
        if (posList != null)
        {
            posList.Clear();

        }
        for (int i = 3; i < transform.childCount; i++)
        {
            posList.Add(transform.GetChild(i));
        }

    }

    private void OnDrawGizmos()
    {
        if (drawPath)
        {
            if (posList.Count > 0)
            {
                for (int i = 0; i < posList.Count - 1; i++)
                    Debug.DrawLine(posList[i].position, posList[i + 1].position, Color.red);
            }
        }


    }
}
