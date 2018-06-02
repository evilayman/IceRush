using AuraAPI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableAura : MonoBehaviour
{
    void Start()
    {
        GetComponent<AuraLight>().enabled = true;
    }
    
}
