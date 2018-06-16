    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(DroneCircleScript))]
[CanEditMultipleObjects]
public class DroneCircleScriptEditor : Editor
{
    SerializedProperty numberOfDrones;
    SerializedProperty radius;

    void OnEnable()
    {
        numberOfDrones = serializedObject.FindProperty("numberOfDrones");
        radius = serializedObject.FindProperty("radius");
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();


        DroneCircleScript myScript = (DroneCircleScript)target;
        if (GUILayout.Button("Apply"))
        {
            myScript.Apply();
            myScript.Update();
        }
        serializedObject.ApplyModifiedProperties();
    }

    //private void OnSceneGUI()
    //{
    //    DroneCircleScript myScript = (DroneCircleScript)target;

    //    myScript.Update();
    //    myScript.GetComponent<ParentPoint>().Update();
    
    //    if (myScript.DroneList != null)
    //    {
    //        for (int i = 0; i < myScript.DroneList.Count; i++)
    //        {
    //            myScript.DroneList[i].GetComponent<DroneVarScript>().Update();
    //        }
    //    }
    //}
}