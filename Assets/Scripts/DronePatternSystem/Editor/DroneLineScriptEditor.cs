using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(DroneLineScript))]
[CanEditMultipleObjects]

public class DroneLineScriptEditor : Editor
{
    SerializedProperty numberOfDrones;
    SerializedProperty increment;

    void OnEnable()
    {
        numberOfDrones = serializedObject.FindProperty("numberOfDrones");
        increment = serializedObject.FindProperty("increment");
       
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        DroneLineScript myScript = (DroneLineScript)target;
        if (GUILayout.Button("Apply"))
        {
            myScript.Apply();
            myScript.Update();
        }
        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        DroneLineScript myScript = (DroneLineScript)target;

        myScript.Update();
        myScript.GetComponent<ParentPoint>().Update();

        if(myScript.DroneList != null)
        {
            for (int i = 0; i < myScript.DroneList.Count; i++)
            {
                myScript.DroneList[i].GetComponent<DroneVarScript>().Update();
            }
        }
    }
}
