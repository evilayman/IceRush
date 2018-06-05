using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDronePattern : MonoBehaviour
{
    public enum MyDangerLevel
    {
        Slow,
        Respwan,
        Death
    }

    [Header("Drone Properties")]
    public GameObject drone;
    public Material myMatSlow, myMatRespwan, myMatDeath;
    public bool enableRayCast;
    [Header("Pattern Movement")]
    public bool move;
    public Vector3 direction;
    //[Space(20)]
    //public Vector3 startPosition;
    //public Vector3 startRotation;

    private List<GameObject> alldroneGO = new List<GameObject>();
    public List<ShapeTransform> shapeTransforms = new List<ShapeTransform>();

    public List<GameObject> AlldroneGO
    {
        get
        {
            return alldroneGO;
        }

        set
        {
            alldroneGO = value;
        }
    }

    public void Start()
    {
        AddChildren();
        ResetPosition();
    }
    public void Update()
    {
        if (move)
        {
            transform.position += (direction * Time.deltaTime);
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "PlayerCollider")
            enableRayCast = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "PlayerCollider")
            enableRayCast = false;
    }
    public void AddLine()
    {
        var go = new GameObject("Line");
        go.transform.SetParent(transform);
        go.transform.position = transform.position;
        DroneLineScript DLS = go.AddComponent<DroneLineScript>();
        DLS.Apply();
        go.AddComponent<ParentPoint>();
        AlldroneGO.Add(go);
        shapeTransforms.Add(new ShapeTransform(Vector3.zero, Vector3.zero));

    }

    public void AddCircle()
    {
        var go = new GameObject("Circle");
        go.transform.SetParent(transform);
        go.transform.position = transform.position;
        DroneCircleScript DCS = go.AddComponent<DroneCircleScript>();
        go.AddComponent<ParentPoint>();
        DCS.Apply();
        AlldroneGO.Add(go);
        shapeTransforms.Add(new ShapeTransform(Vector3.zero, Vector3.zero));
    }

    public void ResetPosition()
    {

        //transform.position = startPosition;
        //transform.rotation = Quaternion.Euler(startRotation);

        if (AlldroneGO != null)
        {
            for (int i = 0; i < AlldroneGO.Count; i++)
            {
                AlldroneGO[i].transform.localPosition = shapeTransforms[i].position ;
                AlldroneGO[i].transform.localEulerAngles = shapeTransforms[i].rotation ;
            }
        }
    }

    public void AutoClean()
    {
        if (transform.childCount > 0 && AlldroneGO.Count == 0)
        {
            AddChildren();
        }

        if (AlldroneGO != null)
        {
            for (int i = 0; i < AlldroneGO.Count; i++)
            {
                if (AlldroneGO[i] == null)
                {
                    shapeTransforms.RemoveAt(i);
                    AlldroneGO.RemoveAt(i);
                }
            }
        }
    }

    public void AddChildren()
    {
        alldroneGO.Clear();
        shapeTransforms.Clear();
        foreach (Transform child in transform)
        {
            AlldroneGO.Add(child.gameObject);
            shapeTransforms.Add(new ShapeTransform(child.position - transform.localPosition, child.eulerAngles - transform.localEulerAngles));
        }
    }
}