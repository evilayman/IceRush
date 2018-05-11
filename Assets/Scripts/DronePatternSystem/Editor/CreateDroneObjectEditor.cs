using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateDronePattern))]
[CanEditMultipleObjects]
public class CreateDroneObjectEditor : Editor
{
    CreateDronePattern myScript;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        myScript = (CreateDronePattern)target;
        myScript.AutoClean();

        if (GUILayout.Button("Add Line"))
        {
            myScript.AddLine();
        }

        if (GUILayout.Button("Add Cricle"))
        {
            myScript.AddCircle();
        }

        if (GUILayout.Button("Reset Position"))
        {
            myScript.ResetPosition();
        }

        serializedObject.ApplyModifiedProperties();
    }   

    public void OnSceneGUI()
    {
        myScript = (CreateDronePattern)target;
        myScript.Update();

        if (myScript.AlldroneGO != null)
        {
            for (int i = 0; i < myScript.AlldroneGO.Count; i++)
            {
                if (myScript.AlldroneGO[i] != null)
                {
                    if (myScript.AlldroneGO[i].GetComponent<DroneLineScript>())
                        myScript.AlldroneGO[i].GetComponent<DroneLineScript>().Update();

                    else
                        myScript.AlldroneGO[i].GetComponent<DroneCircleScript>().Update();
                }
            }
        }
    }
}