using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GoalScript))]
public class RadioButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // 対象のスクリプトを取得
        GoalScript script = (GoalScript)target;

        // Enumの値を取得
        SelectManager.SeasonEnum selected = script.season;

        // GUIでラジオボタン風に描画
        EditorGUILayout.LabelField("解放するSeason");

        foreach (SelectManager.SeasonEnum name in System.Enum.GetValues(typeof(SelectManager.SeasonEnum)))
        {
            if (GUILayout.Toggle(selected == name, name.ToString(), "Radio"))
            {
                selected = name;
            }
        }

        // 変更があった場合は保存
        if (selected != script.season)
        {
            Undo.RecordObject(script, "Change Option");
            script.season = selected;
            EditorUtility.SetDirty(script);
        }
    }
}