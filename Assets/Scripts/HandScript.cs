using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : MonoBehaviour
{

    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    private bool isTriggerPressed = false;

    public bool IsTriggerPressed
    {
        get
        {
            return isTriggerPressed;
        }

        set
        {
            isTriggerPressed = value;
        }
    }

    void Start()
    {
        trackedObject = GetComponentInParent<SteamVR_TrackedObject>();
    }

    void Update()
    {
        device = SteamVR_Controller.Input((int)trackedObject.index);

        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            isTriggerPressed = true;
        }

        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            isTriggerPressed = false;
        }
    }
}
