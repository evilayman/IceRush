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
    private int count = 0;
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
            print(collided);
            player = other.GetComponent<Transform>();
            other.gameObject.GetComponentInParent<Rigidbody>().detectCollisions = false;
            collided = true;
        }
    }


    private void Update()
    {
        if (collided)
        {
            if (upOrDown)
            {
                player.parent.transform.position = Vector3.MoveTowards(player.parent.transform.position, posList[index].position,
                 moveTowardsSpeed * Time.deltaTime);
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
            if (player.position.z == posList[index].position.z && index == PosList.Count - 1)
            {
                player.gameObject.GetComponentInParent<Rigidbody>().detectCollisions = true;
                collided = false;
            }
        }


    }

    public void Move(int i)
    {
        targetDir = posList[i].position - player.parent.transform.position;
        float step = rotateSpeed * Time.deltaTime;
        newDir = Vector3.RotateTowards(player.parent.transform.forward, targetDir, step, 0.0f);
        player.parent.transform.rotation = Quaternion.LookRotation(newDir);

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
        for (int i = 2; i < transform.childCount; i++)
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
