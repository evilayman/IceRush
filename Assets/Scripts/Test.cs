using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    bool myBool = true;

    void Start ()
    {
        testFunction( ((x) => myBool = x) );
	}

    void testFunction(System.Action<bool> myAction)
    {
        StartCoroutine(test(myAction));
    }

    IEnumerator test(System.Action<bool> myAction)
    {
        yield return new WaitForSeconds(1);
        myAction(false);
    }

    private void Update()
    {
        Debug.Log(myBool);
    }

}
