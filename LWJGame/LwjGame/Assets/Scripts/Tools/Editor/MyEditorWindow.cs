using UnityEngine;
using System.Collections;
using UnityEditor;

public class MyEditorWindow : EditorWindow {

    [MenuItem("Window/My Window")]
	static void Excute()
    {
        EditorWindow.GetWindow<MyEditorWindow>();
        Debug.Log("My Window Ceate");
    }

    string myString = "";
    bool myBool = true;
    bool groupEnabled = true;
    float myFloat = 0f;
    private void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -1, 1);
        EditorGUILayout.EndToggleGroup();
    }
}
