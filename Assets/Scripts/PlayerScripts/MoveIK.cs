using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIK : MonoBehaviour
{
    public Transform leftHand, rightHand, head;
    Animator anim;
    Vector3 dir, test;
    Rigidbody rb;
    bool tempM;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponentInParent<Rigidbody>();
    }

    private void Update()
    {
        ChangeAnimation();
    }

    private void ChangeAnimation()
    {
        if (rb.velocity.magnitude > 5)
        {
            if (!tempM)
            {
                anim.SetBool("isMoving", true);
                tempM = true;
            }
        }
        else
        {
            if (tempM)
            {
                anim.SetBool("isMoving", false);
                tempM = false;
            }
        }
    }

    private void OnAnimatorIK()
    {
        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
        anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);

        anim.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
        anim.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
        anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);

        anim.SetLookAtPosition(head.position);
        anim.SetLookAtWeight(1);

        dir = (head.position - transform.position).normalized;
        transform.forward = new Vector3(dir.x, transform.forward.y, dir.z);

    }
}
