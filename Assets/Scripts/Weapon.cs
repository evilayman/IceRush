using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Weapon : VRTK_InteractableObject
{
    private Transform parent;
    private CooldownTimer autoGrabCD;
    private bool forceDroppedUse, forceDroppedNotUsed, forceDroppedUseGrab, useState;

    public void Start()
    {
        parent = GetComponentInParent<Transform>();
        autoGrabCD = new CooldownTimer(1f);
    }

    public override void StartTouching(VRTK_InteractTouch currentTouchingObject = null)
    {
        if (autoGrabCD != null && autoGrabCD.IsReady())
        {
            base.StartTouching(currentTouchingObject);
            VRTK_InteractGrab myGrab = currentTouchingObject.GetComponent<VRTK_InteractGrab>();
            myGrab.AttemptGrab();
        }
    }

    public override void OnInteractableObjectUsed(InteractableObjectEventArgs e)
    {
        base.OnInteractableObjectUsed(e);

        if (forceDroppedUseGrab)
            grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.TriggerPress;

        ForceStopInteracting();
        forceDroppedUse = true;
        forceDroppedUseGrab = false;

    }

    public override void OnInteractableObjectGrabbed(InteractableObjectEventArgs e)
    {
        base.OnInteractableObjectGrabbed(e);

        if (!forcedDropped)
        {
            grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.Undefined;
            ForceStopInteracting();
            forceDroppedUseGrab = true;
        }

        forceDroppedUse = false;

    }

    public override void OnInteractableObjectUngrabbed(InteractableObjectEventArgs e)
    {
        if (!forceDroppedUse && !forceDroppedUseGrab)
        {
            base.OnInteractableObjectUngrabbed(e);
            autoGrabCD.Reset();
        }
    }

}