using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneLineScript : MonoBehaviour
{
    private GameObject drone;
    private Material myMat;
    public float lineWidth = 5f;

    public CreateDronePattern.MyDangerLevel danger;

    [Header("Line Stats")]
    public int numberOfDrones = 2;
    public float increment = 50;
    private List<GameObject> droneList = new List<GameObject>();
    public List<DroneVar> myDroneVarList = new List<DroneVar>();

    private Vector3 Position;
    private float xPos;
    private RaycastHit hit;
    private bool rayCastT;
    private CreateDronePattern pattern;
    
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
        pattern = GetComponentInParent<CreateDronePattern>();
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
            DroneList[j].GetComponent<LineRenderer>().material = myMat;
            DroneList[j].GetComponent<LineRenderer>().startWidth = lineWidth;
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
                    if (pattern && pattern.enableRayCast)
                    {
                        DroneRay(DroneList[i].transform.position, DroneList[i + 1].transform.position);
                    }
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
            if (hit.collider.tag == "PlayerCollider")
            {
                hit.collider.GetComponentInParent<PlayerManagerForNetwork>().DroneHit(danger);
            }
        }
        Debug.DrawRay(start, dir * distance, Color.blue);
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
            case CreateDronePattern.MyDangerLevel.Slow:
                myMat = gameObject.GetComponentInParent<CreateDronePattern>().myMatSlow;
                break;
            case CreateDronePattern.MyDangerLevel.Respwan:
                myMat = gameObject.GetComponentInParent<CreateDronePattern>().myMatRespwan;
                break;
            case CreateDronePattern.MyDangerLevel.Death:
                myMat = gameObject.GetComponentInParent<CreateDronePattern>().myMatDeath;
                break;
            default:
                myMat = gameObject.GetComponentInParent<CreateDronePattern>().myMatSlow;
                break;
        }
    }
}