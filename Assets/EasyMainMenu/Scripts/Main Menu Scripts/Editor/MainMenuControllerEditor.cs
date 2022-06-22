using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MainMenuController))]
public class MainMenuControllerEditor : Editor {

    public override void OnInspectorGUI()
    {
        EMM_AddCanvases.OpenWindow();

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

        EditorGUILayout.LabelField("Main Menu Controller", EditorStyles.helpBox);

        GUIStyle bSKin = new GUIStyle("box");
        bSKin.normal.textColor = Color.green;

       
        EditorGUILayout.EndHorizontal();

      

        base.OnInspectorGUI();

        EditorGUILayout.EndVertical();

        
    }

}
