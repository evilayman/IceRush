using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsShowHide : MonoBehaviour
{
    public Transform Parent;
    private List<Transform> myObjs, myObjsDisable;
    public List<float> myObjsTime, myObjsDisableTime;
    public float startPosition, speed, areaSize;

    void Start()
    {
        myObjs = myObjsDisable = new List<Transform>();
        myObjsTime = myObjsDisableTime = new List<float>();

        GetAllObjects();
    }

    void GetAllObjects()
    {
        for (int i = 0; i < Parent.childCount; i++)
        {
            GetChildren((Parent.GetChild(i).transform));
        }

        myObjs.Sort(CompareTransform);
        myObjsTime = SetActiveTime(myObjs);
    }

    void GetChildren(Transform Parent)
    {
        for (int i = 0; i < Parent.childCount; i++)
        {
            myObjs.Add(Parent.GetChild(i).transform);
            Parent.GetChild(i).transform.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        CheckTimeActive(0);
        //CheckTimeInActive(0);
    }

    void CheckTimeActive(int index)
    {
        if (index < myObjsTime.Count && myObjsTime.Count > 0 && Time.time >= myObjsTime[index])
        {
            myObjs[index].gameObject.SetActive(true);

            StartCoroutine(DeActivateObj(myObjs[index], (areaSize / speed) + Time.time));

            //myObjsDisable.Add(myObjs[index].transform);
            //myObjsDisableTime.Add((areaSize / speed) + Time.time);

            myObjs.RemoveAt(index);
            myObjsTime.RemoveAt(index);

            CheckTimeActive(index + 1);
        }
    }

    //void CheckTimeInActive(int index)
    //{
    //    if (index < myObjsDisableTime.Count && myObjsDisableTime.Count > 0 && Time.time >= myObjsDisableTime[index])
    //    {
    //        myObjsDisable[index].gameObject.SetActive(false);

    //        myObjsDisable.RemoveAt(index);
    //        myObjsDisableTime.RemoveAt(index);

    //        CheckTimeInActive(index + 1);
    //    }
    //}

    private List<float> SetActiveTime(List<Transform> myObjs)
    {
        List<float> temp = new List<float>();
        float time;

        for (int i = 0; i < myObjs.Count; i++)
        {
            time = (myObjs[i].transform.position.z - startPosition) / speed; // Make Time Calculation Here
            temp.Add(time);
        }

        return temp;
    }

    private int CompareTransform(Transform A, Transform B)
    {
        return A.transform.position.z.CompareTo(B.transform.position.z);
    }

    IEnumerator DeActivateObj(Transform Obj, float time)
    {
        yield return new WaitForSeconds(time);
        Obj.gameObject.SetActive(false);
    }
}