using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightHand : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    Movement MovementScript;
    public ParticleSystem Emission;

    void Start()
    {

        trackedObject = GetComponentInParent<SteamVR_TrackedObject>();
        MovementScript = GameObject.Find("Player").GetComponent<Movement>();

    }

    void Update()
    {

        device = SteamVR_Controller.Input((int)trackedObject.index);
        if (device.GetPressDown(SteamVR_Controller.ButtonMask.Trigger))
        {
            MovementScript.IsPressedRight = true;
            device.TriggerHapticPulse(1000);
            if(!Emission.isPlaying)
                Emission.Play();

        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            MovementScript.IsPressedRight = false;
            if (Emission.isPlaying)
                Emission.Stop();

        }
    }
}
