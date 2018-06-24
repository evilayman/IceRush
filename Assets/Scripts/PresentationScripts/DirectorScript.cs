using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DirectorScript : MonoBehaviour
{
    public MoveTarget LookAtTarget;
    public GameObject slides;
    public GameObject cams;

    private List<GameObject> slidesList = new List<GameObject>();
    private List<CinemachineVirtualCamera> camsList = new List<CinemachineVirtualCamera>();

    private int slideNumber = 0, currentPriority = 0;

    private TextMeshPro[] myTexts;

    void Start()
    {
        myTexts = FindObjectsOfType<TextMeshPro>();
        Init();
    }

    private void Init()
    {
        for (int i = 0; i < slides.transform.childCount; i++)
        {   
            slidesList.Add(slides.transform.GetChild(i).gameObject);
        }
        for (int i = 0; i < cams.transform.childCount; i++)
        {
            camsList.Add(cams.transform.GetChild(i).GetComponent<CinemachineVirtualCamera>());
        }

        SetSlide(slideNumber);

    }

    void Update()
    {
        SwitchSlide();
        RotateText();
    }

    private void RotateText()
    {
        for (int i = 0; i < myTexts.Length; i++)
        {
            if (myTexts[i].IsActive())
            {
                Vector3 newDir = Vector3.RotateTowards(myTexts[i].transform.forward, camsList[currentPriority].transform.forward, 0.5f * Time.deltaTime, 0.0f);
                myTexts[i].transform.rotation = Quaternion.LookRotation(newDir);
            }
        }
    }
    private void CameraMove()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentPriority == camsList.Count - 1)
            {
                currentPriority = 0;
            }
            else
            {
                currentPriority++;
            }
            SetPrio(currentPriority);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentPriority == 0)
            {
                currentPriority = camsList.Count - 1;
            }
            else
            {
                currentPriority--;
            }

            SetPrio(currentPriority);
        }
    }
    private void SwitchSlide()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (slideNumber == slidesList.Count - 1)
            {
                slideNumber = 0;
            }
            else
            {
                slideNumber++;
            }
            SetSlide(slideNumber);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (slideNumber == 0)
            {
                slideNumber = slidesList.Count - 1;
            }
            else
            {
                slideNumber--;
            }

            SetSlide(slideNumber);
        }
    }
    public void SetCam(int index)
    {
        SetPrio(index);
    }
    private void SetPrio(int index)
    {
        for (int i = 0; i < camsList.Count; i++)
        {
            if (i == index)
                camsList[i].Priority = 11;
            else
                camsList[i].Priority = 10;
        }
    }

    private void ActivateSlide(int index)
    {
        for (int i = 0; i < slidesList.Count; i++)
        {
            slidesList[i].SetActive(i == index);
        }
    }

    private void SetSlide(int slideNumber)
    {
        ActivateSlide(slideNumber);
        LookAtTarget.target = slidesList[slideNumber].transform.position;
        Debug.Log(LookAtTarget.target);
        SetCam(0);

        if (slideNumber == 1)
        {
            SetCam(1);
        }

    }
}
