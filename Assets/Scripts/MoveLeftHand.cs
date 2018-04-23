using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftHand : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    Movement MovementScript;
    public ParticleSystem Emission;

    private bool isPressedLeft = false;

    public bool IsPressedLeft
    {
        get
        {
            return isPressedLeft;
        }

        set
        {
            isPressedLeft = value;
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
            isPressedLeft = true;
            device.TriggerHapticPulse(1000);
            if (!Emission.isPlaying)
                Emission.Play();

        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            isPressedLeft = false;
            if (Emission.isPlaying)
                Emission.Stop();

        }
    }
}
