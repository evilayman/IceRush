using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleOvertime : MonoBehaviour
{
	void FixedUpdate ()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 0.03f);
	}
}
