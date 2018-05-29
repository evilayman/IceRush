using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroneCircleScript : MonoBehaviour
{
    private GameObject drone;
    private Material myMat;
    public float lineWidth = 1f;

    public CreateDronePattern.MyDangerLevel danger;

    [Header("Circle Stats")]
    public int numberOfDrones = 4;
    public float radius = 5;
    public bool hasCenter = true, hasEdges = true;

    private List<GameObject> droneList = new List<GameObject>();
    public List<DroneVar> myDroneVarList = new List<DroneVar>();
    private List<GameObject> centerDroneGOList = new List<GameObject>();

    private RaycastHit hit;
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
        AddDrones();
        pattern = GetComponentInParent<CreateDronePattern>();

        //drone = gameObject.GetComponentInParent<CreateDronePattern>().drone;
        //myMat = gameObject.GetComponentInParent<CreateDronePattern>().lineRendererMaterial;
    }

    public void Update()
    {
        UpdateCircle();
    }

    public void CreateCircle()
    {
        for (int i = 0; i < numberOfDrones + 1; i++)
        {
            if (i == 0)
            {
                CreateDrone(transform.position, i);
                CreateCenterGO();
            }
            else
            {
                float ang = i * (360 / numberOfDrones);
                Vector3 pos = RandomCircle(transform.position, radius, ang);
                CreateDrone(pos, i);
            }
        }
    }

    public void CreateCenterGO()
    {
        for (int i = 0; i < numberOfDrones; i++)
        {
            GameObject go = new GameObject();
            go.transform.SetParent(DroneList[0].transform);
            go.AddComponent<LineRenderer>().material = myMat;
            go.GetComponent<LineRenderer>().startWidth = lineWidth;
            centerDroneGOList.Add(go);
        }
    }

    void CreateDrone(Vector3 position, int i)
    {
        GameObject go = Instantiate(drone, position, Quaternion.identity, transform);
        droneList.Add(go);

        go.name = i + "";

        if (hasEdges && i != 0)
        {
            go.GetComponent<LineRenderer>().material = myMat;
            go.GetComponent<LineRenderer>().startWidth = lineWidth;
        }

        else if (!hasCenter && i == 0)
            droneList[i].SetActive(false);

        go.AddComponent<DroneVarScript>();
        myDroneVarList.Add(go.GetComponent<DroneVarScript>().myDronVar);
    }

    Vector3 RandomCircle(Vector3 center, float radius, float ang)
    {
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
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
            centerDroneGOList.Clear();
            myDroneVarList.Clear();
        }

        CreateCircle();
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

    private void UpdateCircle()
    {
        AutoClean();

        if (DroneList != null && DroneList.Count >= 2)
        {
            for (int i = 1; i < DroneList.Count; i++)
            {
                if (hasEdges)
                    ConnectEdges(i);
                if (hasCenter)
                    ConnectCenter(i);

                DroneList[i].GetComponent<DroneVarScript>().myDronVar = myDroneVarList[i];
            }
        }
    }

    private void ConnectEdges(int i)
    {
        if (i < DroneList.Count - 1 && DroneList[i].GetComponent<LineRenderer>())
        {
            DroneList[i].GetComponent<LineRenderer>().SetPosition(0, DroneList[i].transform.position);
            DroneList[i].GetComponent<LineRenderer>().SetPosition(1, DroneList[i + 1].transform.position);
            if (pattern && pattern.enableRayCast)
            {
                DroneRay(DroneList[i].transform.position, DroneList[i + 1].transform.position);
            }
        }
        else if (DroneList[i].GetComponent<LineRenderer>())
        {
            DroneList[i].GetComponent<LineRenderer>().SetPosition(0, DroneList[i].transform.position);
            DroneList[i].GetComponent<LineRenderer>().SetPosition(1, DroneList[1].transform.position);
            if (pattern && pattern.enableRayCast)
            {
                DroneRay(DroneList[i].transform.position, DroneList[1].transform.position);
            }
        }
    }

    private void ConnectCenter(int i)
    {
        centerDroneGOList[i - 1].GetComponent<LineRenderer>().SetPosition(0, DroneList[0].transform.position);
        centerDroneGOList[i - 1].GetComponent<LineRenderer>().SetPosition(1, DroneList[i].transform.position);
        if (pattern && pattern.enableRayCast)
        {
            DroneRay(DroneList[0].transform.position, DroneList[i].transform.position);
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

    private void AutoClean()
    {
        if (transform.childCount > 0 && DroneList.Count == 0)
        {
            AddDrones();
        }
        if (DroneList[0] && DroneList[0].transform.childCount > 0 && centerDroneGOList.Count == 0)
        {
            AddCenterDrones();
        }

        if (DroneList != null)
        {
            for (int i = 0; i < DroneList.Count; i++)
            {
                if (DroneList[i] == null)
                {
                    DroneList.RemoveAt(i);
                }
            }
        }

        if (centerDroneGOList != null)
        {
            for (int i = 0; i < centerDroneGOList.Count; i++)
            {
                if (centerDroneGOList[i] == null)
                {
                    centerDroneGOList.RemoveAt(i);
                }
            }
        }
    }

    public void AddCenterDrones()
    {
        //centerDroneGOList.Clear();
        foreach (Transform child in DroneList[0].transform)
        {
            centerDroneGOList.Add(child.gameObject);
        }
    }
}