using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneLineScript : MonoBehaviour
{
    private GameObject drone;
    private Material myMat;

    public MyDangerLevel danger;

    public enum MyDangerLevel
    {
        Slow,
        Respwan,
        Death
    }

    [Header("Line Stats")]
    public int numberOfDrones = 2;
    public float increment = 20;
    private List<GameObject> droneList = new List<GameObject>();
    public List<DroneVar> myDroneVarList = new List<DroneVar>();

    private Vector3 Position;
    private float xPos;
    private RaycastHit hit;

    public List<GameObject> DroneList
    {
        get
        {
            return droneList;
        }
        set
        {
            droneList = value;
        }
    }

    public void Start()
    {
        AddDrones();
    }

    public void Update()
    {
        UpdateDrones();
    }

    public void CreateLine()
    {
        xPos = 0;
        float length = (numberOfDrones - 1) * increment;

        for (int i = 0; i < numberOfDrones; i++)
        {
            Position = new Vector3(xPos - (length / 2), 0, 0);
            var go = Instantiate(drone) as GameObject;
            go.transform.SetParent(transform);
            go.transform.localPosition = Position;
            DroneList.Add(go);
            xPos += increment;
        }

        for (int j = 0; j < (DroneList.Count - 1); j++)
        {
            DroneList[j].AddComponent<LineRenderer>().material = myMat;

        }

        for (int i = 0; i < DroneList.Count; i++)
        {
            DroneList[i].name = i + "";
            DroneList[i].AddComponent<DroneVarScript>();

            myDroneVarList.Add(DroneList[i].GetComponent<DroneVarScript>().myDronVar);
        }
    }

    public void Apply()
    {
        SetDroneMat();

        if (DroneList != null)
        {
            for (int i = 0; i < DroneList.Count; i++)
            {
                DestroyImmediate(DroneList[i]);
            }
            DroneList.Clear();
            myDroneVarList.Clear();
        }

        CreateLine();
    }

    private void UpdateDrones()
    {
        if (transform.childCount > 0 && DroneList.Count == 0)
        {
            AddDrones();
        }

        if (DroneList != null && DroneList.Count >= 2)
        {
            for (int i = 0; i < DroneList.Count; i++)
            {
                if (i < DroneList.Count - 1)
                {
                    DroneList[i].GetComponent<LineRenderer>().SetPosition(0, DroneList[i].transform.position);
                    DroneList[i].GetComponent<LineRenderer>().SetPosition(1, DroneList[i + 1].transform.position);

                    DroneRay(DroneList[i].transform.position, DroneList[i + 1].transform.position);
                }
                DroneList[i].GetComponent<DroneVarScript>().myDronVar = myDroneVarList[i];
            }
        }
    }

    private void DroneRay(Vector3 start, Vector3 end)
    {
        var dir = (end - start).normalized;
        Ray ray = new Ray(start, dir);
        var distance = Vector3.Distance(start, end);

        if (Physics.Raycast(ray, out hit, distance))
        {
            if (hit.collider.tag == "Player")
            {
                RayHit(hit.collider.gameObject);
            }
        }
        Debug.DrawRay(start, dir * distance, Color.blue);
    }

    private void RayHit(GameObject player)
    {
        switch (danger)
        {
            case MyDangerLevel.Slow:
                break;
            case MyDangerLevel.Respwan:
                break;
            case MyDangerLevel.Death:
                break;
            default:
                break;
        }
    }

    public void AddDrones()
    {
        foreach (Transform child in transform)
        {
            DroneList.Add(child.gameObject);
        }
    }

    private void SetDroneMat()
    {
        drone = gameObject.GetComponentInParent<CreateDronePattern>().drone;

        switch (danger)
        {
            case MyDangerLevel.Slow:
                myMat = gameObject.GetComponentInParent<CreateDronePattern>().myMatSlow;
                break;
            case MyDangerLevel.Respwan:
                myMat = gameObject.GetComponentInParent<CreateDronePattern>().myMatRespwan;
                break;
            case MyDangerLevel.Death:
                myMat = gameObject.GetComponentInParent<CreateDronePattern>().myMatDeath;
                break;
            default:
                myMat = gameObject.GetComponentInParent<CreateDronePattern>().myMatSlow;
                break;
        }
    }
}