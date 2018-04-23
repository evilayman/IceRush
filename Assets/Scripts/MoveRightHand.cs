using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRightHand : MonoBehaviour
{
    private SteamVR_TrackedObject trackedObject;
    private SteamVR_Controller.Device device;

    Movement MovementScript;
    public ParticleSystem Emission;
    private bool isPressedRight = false;
    public bool IsPressedRight
    {
        get
        {
            return isPressedRight;
        }

        set
        {
            isPressedRight = value;
        }
    }
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
            //how much is the trigger pressed still working on it..
            //Debug.Log(device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).x);
            //Debug.Log(device.GetAxis(Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger).y);
            isPressedRight = true;
            device.TriggerHapticPulse(1000);
            if(!Emission.isPlaying)
                Emission.Play();
            

        }
        if (device.GetPressUp(SteamVR_Controller.ButtonMask.Trigger))
        {
            isPressedRight = false;
            if (Emission.isPlaying)
                Emission.Stop();
        }
    }
}
