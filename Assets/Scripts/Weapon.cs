using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Weapon : VRTK_InteractableObject
{
    private Transform parent;

    public void Start()
    {
        parent = GetComponentInParent<Transform>();
    }

    public override void StartTouching(VRTK_InteractTouch currentTouchingObject = null)
    {
        base.StartTouching(currentTouchingObject);
        VRTK_InteractGrab myGrab = currentTouchingObject.GetComponent<VRTK_InteractGrab>();
        myGrab.AttemptGrab();
    }

    protected override void Update()
    {
        base.Update();
    }

}
