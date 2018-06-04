using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleModel : MonoBehaviour
{

    public Transform targetHeight, headset, model;

    void Start()
    {
        StartCoroutine(Slow(1));
    }

    private IEnumerator Slow(float time)
    {
        yield return new WaitForSeconds(time);

        model.localScale = Vector3.one * (headset.position.y / targetHeight.transform.localPosition.y);
    }

}
