using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsShowHide : MonoBehaviour
{
    public Transform gameRegion, enviroment;

    private List<Transform> myObjs;
    private List<float> myObjsTime;

    private GameRegion regionScript;
    
    void Start()
    {
        regionScript = gameRegion.GetComponent<GameRegion>();
        myObjs = new List<Transform>();
        myObjsTime = new List<float>();

        GetAllObjects();
    }

    void GetAllObjects()
    {
        for (int i = 0; i < enviroment.childCount; i++)
        {
            GetChildren((enviroment.GetChild(i).transform));
        }

        myObjs.Sort(CompareTransform);
        myObjsTime = SetActiveTime(myObjs);
    }

    void GetChildren(Transform parent)
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
            myObjs[index].gameObject.SetActive(true);

            StartCoroutine(DeActivateObj(myObjs[index], (regionScript.regionSize / regionScript.speed)));

            myObjs.RemoveAt(index);
            myObjsTime.RemoveAt(index);

            CheckToEnable(index + 1);
        }
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