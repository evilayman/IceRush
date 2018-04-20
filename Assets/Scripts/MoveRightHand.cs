using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightHand : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    ApplyForce AFScript;
    public ParticleSystem Emission;

    void Start()
    {

        trackedObject = GetComponentInParent<SteamVR_TrackedObject>();
        AFScript = GameObject.Find("Player").GetComponent<ApplyForce>();

    }

    void Update()
    {

        device = SteamVR_Controller.Input((int)trackedObject.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            AFScript.IsPressedRight = true;
            device.TriggerHapticPulse(1000);
            if(!Emission.isPlaying)
                Emission.Play();

        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            AFScript.IsPressedRight = false;
            if (Emission.isPlaying)
                Emission.Stop();

        }
    }
}
