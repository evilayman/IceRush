﻿using Cinemachine;
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
                Vector3 newDir = Vector3.RotateTowards(myTexts[i].transform.forward, Camera.main.transform.forward, 0.5f * Time.deltaTime, 0.0f);
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
        LookAtTarget.target = slidesList[slideNumber].transform.GetChild(0).position;

        SetCam(0);
        
        switch (slideNumber)
        {
            case 1:
                SetCam(1);
                break;
            case 2:
                SetCam(2);
                break;
            case 3:
                SetCam(3);
                break;
            case 4:
                SetCam(4);
                break;
            case 5 :
                SetCam(5);
                break;
            case 6 :
                SetCam(6);
                break;
            case 7 :
                SetCam(7);
                break;
            case 8 :
                SetCam(8);
                break;
            case 9 :
                SetCam(9);
                break;
            case 10 :
                SetCam(10);
                break;
            case 11 :
                SetCam(11);
                break;
            case 12 :
                SetCam(12);
                break;
            default:
                break;
        }
    }
}
