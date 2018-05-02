using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Weapon : VRTK_InteractableObject
{

    public override void StartTouching(VRTK_InteractTouch currentTouchingObject = null)
    {
        base.StartTouching(currentTouchingObject);
        VRTK_InteractGrab myGrab = currentTouchingObject.GetComponent<VRTK_InteractGrab>();
        myGrab.AttemptGrab();
    }





}
