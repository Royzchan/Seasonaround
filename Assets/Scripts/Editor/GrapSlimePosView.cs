// エディタ拡張
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GrapGimmick))]
public class PositionHolderEditor : Editor
{
    private void OnSceneGUI()
    {
        GrapGimmick grap = (GrapGimmick)target;
        Handles.color = Color.green;

        // シーンビューで球体を表示
        Handles.SphereHandleCap(0, grap.transform.position + grap.transform.rotation*grap._grapPos, Quaternion.identity, 0.5f, EventType.Repaint);
        
        //位置を調整できるようにする
        //EditorGUI.BeginChangeCheck();
        //Vector3 newTargetPosition = Handles.PositionHandle(grap._grapPos + grap.transform.parent.position, Quaternion.identity);
        //if (EditorGUI.EndChangeCheck())
        //{
        //    Undo.RecordObject(grap, "Move Target Position");
        //    grap._grapPos = newTargetPosition - grap.transform.parent.position;
        //}
    }
}