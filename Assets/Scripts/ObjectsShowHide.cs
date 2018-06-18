using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsShowHide : MonoBehaviour
{
    public Transform gameRegion, enviroment;
    public float borderSize;

    private List<Transform> myObjs;
    private List<float> myObjsTime;

    private GameRegion regionScript;

    void Start()
    {
        regionScript = gameRegion.GetComponent<GameRegion>();
        myObjs = new List<Transform>();
        myObjsTime = new List<float>();

        GetRegions(enviroment);
        myObjs.Sort(CompareTransform);
        myObjsTime = SetActiveTime(myObjs);
    }

    void GetRegions(Transform parent)
    {
        for (int i = 1; i < parent.childCount; i++)
        {
            GetGroups((parent.GetChild(i).transform));
        }
    }

    void GetGroups(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            AddChildren((parent.GetChild(i).transform));
        }
    }

    void AddChildren(Transform parent)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            myObjs.Add(parent.GetChild(i).transform);
            parent.GetChild(i).transform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        CheckToEnable(0);
    }

    void CheckToEnable(int index)
    {
        if (index < myObjsTime.Count && myObjsTime.Count > 0 && Time.time >= myObjsTime[index])
        {
            if (myObjs[index].gameObject.tag == "Drone")
            {
                StartCoroutine(ActivateDrone(myObjs[index]));
            }
            else
            {
                myObjs[index].gameObject.SetActive(true);
                StartCoroutine(DeActivateObj(myObjs[index], ((regionScript.regionSize) / (regionScript.speed))));
            }

            myObjs.RemoveAt(index);
            myObjsTime.RemoveAt(index);

            CheckToEnable(index + 1);
        }
    }

    IEnumerator ActivateDrone(Transform Obj)
    {
        var delay = ((regionScript.regionSize/2) - (borderSize/2)) / regionScript.speed;
        yield return new WaitForSeconds(delay);
        Obj.gameObject.SetActive(true);
        var objSpeed = -Obj.gameObject.GetComponent<CreateDronePattern>().direction.z;
        StartCoroutine(DeActivateObj(Obj, ((regionScript.regionSize) / (regionScript.speed - objSpeed))));
    }

    IEnumerator DeActivateObj(Transform Obj, float time)
    {
        yield return new WaitForSeconds(time);
        Obj.gameObject.SetActive(false);
    }

    private List<float> SetActiveTime(List<Transform> myObjs)
    {
        List<float> temp = new List<float>();

        float time;
        float regionStartPosition = gameRegion.position.z + (regionScript.regionSize / 2);

        for (int i = 0; i < myObjs.Count; i++)
        {
            time = (myObjs[i].transform.position.z - regionStartPosition) / regionScript.speed;
            temp.Add(time);
        }

        return temp;
    }

    private int CompareTransform(Transform A, Transform B)
    {
        return A.transform.position.z.CompareTo(B.transform.position.z);
    }
}