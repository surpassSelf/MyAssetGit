using UnityEngine;
using System.Collections;
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(UIGridList), true)]

public class UIGridListEditor : UIWidgetContainerEditor
{
    UIGridList mGridList;
    private void OnEnable()
    {
        mGridList = target as UIGridList;
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.LabelField("排列方式");
        mGridList.mArrangeType = (UIGridList.ArrangeMentType)EditorGUILayout.EnumPopup("mArrangeType", mGridList.mArrangeType);
        EditorGUILayout.LabelField("每行或者每列显示的最大个数");
        mGridList.mnMaxLine = EditorGUILayout.IntField("MaxLine", mGridList.mnMaxLine);
        EditorGUILayout.LabelField("锚点");
        mGridList.mPivot = (UIWidget.Pivot)EditorGUILayout.EnumPopup("Pivot", mGridList.mPivot);
        EditorGUILayout.LabelField("克隆的最大数量");
        mGridList.mnMaxCount = EditorGUILayout.IntField("MaxCount", mGridList.mnMaxCount);
        EditorGUILayout.LabelField("每个物体的最大宽度");
        mGridList.mnCellWidth = EditorGUILayout.IntField("CellWidth", mGridList.mnCellWidth);
        EditorGUILayout.LabelField("每个物体的最大长度");
        mGridList.mnCellHeight = EditorGUILayout.IntField("CellHeight", mGridList.mnCellHeight);

        DrawDefaultInspector();
    }
}
