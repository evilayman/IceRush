using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceManMovement : MonoBehaviour
{
    public float baseSpeed;
    public float maxHorizontalSpeed;
    public float maxVerticalSpeed;
    public GameObject player;
    List<Vector3> mousePos;
    Vector3 velocity;
    float mouseMovedX;
    float mouseMovedY;

    private Rigidbody playerRB;
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;



    void Start()
    {
        mousePos = new List<Vector3>();
        baseSpeed = 10;
        velocity = new Vector3(0, 0, 1);

        trackedObject = GetComponentInParent<SteamVR_TrackedObject>();
    }


    void Update()
    {
        MoveJo();

    }
    void MoveJo()
    {
        //mousePos = Input.mousePosition;
        //worldPos = camera.ScreenToWorldPoint(mousePos);
        device = SteamVR_Controller.Input((int)trackedObject.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            mousePos.Add(Input.mousePosition);
            mouseMovedX = mousePos[0].x - mousePos[mousePos.Count - 1].x;
            mouseMovedY = mousePos[0].y - mousePos[mousePos.Count - 1].y;
            if (Mathf.Abs(mouseMovedX) > maxHorizontalSpeed)
            {
                if (mouseMovedX > 0)
                {
                    mouseMovedX = maxHorizontalSpeed;
                }
                else
                {
                    mouseMovedX = -maxHorizontalSpeed;
                }

            }

            if (Mathf.Abs(mouseMovedY) > maxVerticalSpeed)
            {
                if (mouseMovedY > 0)
                {
                    mouseMovedY = maxVerticalSpeed;
                }
                else
                {
                    mouseMovedY = -maxVerticalSpeed;
                }

            }
            velocity = new Vector3(-mouseMovedX, -mouseMovedY, 1);

            if (mousePos.Count > 2)
            {
                mousePos.Clear();
            }



        }
        if (Input.GetMouseButtonUp(0))
        {
            mousePos.Clear();
            velocity = new Vector3(0, 0, 1);
        }

        player.GetComponent<Rigidbody>().velocity = velocity * baseSpeed;

    }
    //    if (Input.GetMouseButton(0))
    //    {
    //        mousePos.Add(Input.mousePosition);
    //        mouseMovedX = mousePos[0].x - mousePos[mousePos.Count - 1].x;
    //        mouseMovedY = mousePos[0].y - mousePos[mousePos.Count - 1].y;          
    //        if (Mathf.Abs(mouseMovedX)>maxHorizontalSpeed)
    //        {
    //            if (mouseMovedX>0)
    //            {
    //                mouseMovedX = maxHorizontalSpeed;
    //            }
    //            else
    //            {
    //                mouseMovedX = -maxHorizontalSpeed;
    //            }

    //        }

    //        if (Mathf.Abs(mouseMovedY) > maxVerticalSpeed)
    //        {
    //            if (mouseMovedY > 0)
    //            {
    //                mouseMovedY = maxVerticalSpeed;
    //            }
    //            else
    //            {
    //                mouseMovedY = -maxVerticalSpeed;
    //            }

    //        }
    //        velocity = new Vector3(-mouseMovedX, -mouseMovedY, 1);

    //        if (mousePos.Count > 2)
    //        {
    //            mousePos.Clear();
    //        }



    //    }
    //    if (Input.GetMouseButtonUp(0))
    //    {
    //        mousePos.Clear();
    //        velocity = new Vector3(0, 0, 1);
    //    }

    //    player.GetComponent<Rigidbody>().velocity = velocity * baseSpeed;

    //}
}
