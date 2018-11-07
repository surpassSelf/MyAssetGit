using UnityEngine;
using System.Collections;
using UnityEditor;

[CanEditMultipleObjects]
[CustomEditor(typeof(LookAtPoint), true)]
public class MyInspector : Editor {

    private LookAtPoint lookAtPoint;
    
    private void OnEnable()
    {
        lookAtPoint = (LookAtPoint)target;
    }

    public override void OnInspectorGUI()
    {
        lookAtPoint.targetPoint = EditorGUILayout.Vector3Field("TargetPoint", lookAtPoint.targetPoint);
        lookAtPoint.radius = EditorGUILayout.FloatField("Radius", lookAtPoint.radius);
    }

    private void OnSceneGUI()
    {
        Handles.BeginGUI();
        if (GUILayout.Button("哈哈哈哈"))
        {
            Debug.Log("!!!!!!!!!!!!!!!!");
        }
        Handles.EndGUI();

        if (lookAtPoint != null)
        {
            Handles.Label(lookAtPoint.transform.position + new Vector3(0, 1, 0), "My Inspector Code");
            lookAtPoint.targetPoint = Handles.PositionHandle(lookAtPoint.targetPoint, Quaternion.identity);
            lookAtPoint.radius = Handles.RadiusHandle(Quaternion.identity, lookAtPoint.transform.position, lookAtPoint.radius);
            Debug.DrawLine(lookAtPoint.transform.position, lookAtPoint.transform.position + new Vector3(0, 10, 0), Color.red);
            Debug.DrawRay(lookAtPoint.transform.position, Vector3.zero, Color.red);
        }
    }
}
