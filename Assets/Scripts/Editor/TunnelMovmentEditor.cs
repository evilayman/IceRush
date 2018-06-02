using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(TunnelMovment))]
public class NewBehaviourScript : Editor {

   

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        serializedObject.Update();

        TunnelMovment myScript = (TunnelMovment)target;
        myScript.AddChildren();
        if (GUILayout.Button("Add New Point"))
        {
            myScript.AddPoint();
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void OnSceneGUI()
    {
        
    }
}
