using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;

public enum Kind
{
    None,
    Vertices,
    Normals
}

public class MeshChange : EditorWindow
{
    private GameObject m_Obj;

    private Vector3 m_ChangedCenterPos = Vector3.zero;
    private Vector3 m_ChangeScale = Vector3.one;
    private Vector3 m_ChangeRotate = Vector3.zero;
    private Vector3[] m_Vertices;
    private Vector3[] m_Normals;

    private static EditorWindow m_Window;

    [MenuItem("GameObject/坐标轴修改")]
    public static void CreatWindow()
    {
        Rect wr = new Rect(150, 150, 320, 200);
        m_Window = EditorWindow.GetWindowWithRect(typeof(MeshChange), wr, false, "坐标轴修改");
        m_Window.Show();
    }

    private void OnEnable()
    {  
        m_Obj = Selection.activeGameObject;
        GetVertices();
    }

    private void OnSelectionChange()
    {
        Init();
        m_Obj = Selection.activeGameObject;
        GetVertices();
        m_Window.Repaint();
    }

    private void OnGUI()
    {
        ModelChange();
    }

    private void GetVertices()
    {      
        if (m_Obj)
        {
            if (m_Obj.GetComponent<MeshFilter>())
            {
                Mesh mesh = m_Obj.GetComponent<MeshFilter>().sharedMesh;
                if (mesh == null) return;
                m_Vertices = new Vector3[mesh.vertices.Length];      
                mesh.vertices.CopyTo(m_Vertices, 0);
                m_Normals = new Vector3[mesh.normals.Length];
                mesh.normals.CopyTo(m_Normals, 0);
            }    
        }
    }

    private void ModelChange()
    {
        if (m_Obj != null)
        {
            if (!m_Obj.GetComponent<MeshFilter>()) return;
            Mesh mesh = m_Obj.GetComponent<MeshFilter>().sharedMesh;
            if (mesh == null) return;
            Vector3 centerPosTemp = Vector3.zero;
            centerPosTemp = mesh.bounds.center;            
            EditorGUILayout.LabelField("CenterPosition", centerPosTemp.ToString(), GUILayout.Width(300));
            m_ChangedCenterPos = EditorGUILayout.Vector3Field("Position", m_ChangedCenterPos, GUILayout.Width(300));
            m_ChangeRotate = EditorGUILayout.Vector3Field("Rotation", m_ChangeRotate, GUILayout.Width(300));
            m_ChangeScale = EditorGUILayout.Vector3Field("Scale", m_ChangeScale, GUILayout.Width(300));
            if (GUILayout.Button("修改", GUILayout.Width(100)))
            {
                mesh.vertices = TRSMethod(Kind.Vertices, mesh, mesh.vertices, m_ChangedCenterPos, m_ChangeRotate, m_ChangeScale);
                mesh.normals = TRSMethod(Kind.Normals, mesh, mesh.normals, m_ChangedCenterPos, m_ChangeRotate, m_ChangeScale);
                mesh.RecalculateBounds();
                //mesh.RecalculateTangents();
                m_Window.Repaint();
            }
            if (GUILayout.Button("保存", GUILayout.Width(100)))
            {
                Save(mesh);
            }
            if (GUILayout.Button("重置", GUILayout.Width(100)))
            {
                mesh.vertices = m_Vertices;
                mesh.normals = m_Normals;
                Save(mesh);
                m_Window.Repaint();
            }
        }
    }

    private Vector3[] TRSMethod(Kind kind, Mesh mesh, Vector3[] vertices, Vector3 destination, Vector3 angle, Vector3 scale)
    {
        Vector3 center = mesh.bounds.center;
        Vector3 dir = (destination - center).normalized;
        float distance = Vector3.Distance(center, destination);
        for (int i = 0; i < vertices.Length; i++)
        {
            Matrix4x4 m = Matrix4x4.identity;
            m.SetTRS(dir * distance, Quaternion.Euler(angle), scale);
            Vector3 temp = Vector3.zero;
            switch (kind)
            {
                case Kind.Vertices:
                    temp = m.MultiplyPoint(vertices[i]);
                    break;
                case Kind.Normals:
                    temp = m.MultiplyVector(vertices[i]);
                    break;
            }        
            vertices[i] = temp;
        }
        return vertices;
    }
    private void Save(Mesh mesh)
    {
        Init();
        Mesh m = Object.Instantiate(mesh);
        string path = Application.dataPath;
        if (!Directory.Exists(path + "/ChangedMesh"))
        {
            Directory.CreateDirectory(path + "/ChangedMesh");
        }
        AssetDatabase.CreateAsset(m, "Assets/ChangedMesh/" + m_Obj.name + ".asset");
        AssetDatabase.Refresh();
        m_Obj.GetComponent<MeshFilter>().mesh = AssetDatabase.LoadAssetAtPath("Assets/ChangedMesh/" + m_Obj.name + ".asset", typeof(Mesh)) as Mesh;
    }

    private void Init()
    {
        m_ChangedCenterPos = Vector3.zero;
        m_ChangeScale = Vector3.one;
        m_ChangeRotate = Vector3.zero;
    }

    private void OnDestroy()
    {
        Debug.LogError("MeshChange Close");
    }
}
