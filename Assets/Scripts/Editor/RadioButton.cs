using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GoalScript))]
public class RadioButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // �Ώۂ̃X�N���v�g���擾
        GoalScript script = (GoalScript)target;

        // Enum�̒l���擾
        SelectManager.SeasonEnum selected = script.season;

        // GUI�Ń��W�I�{�^�����ɕ`��
        EditorGUILayout.LabelField("�������Season");

        foreach (SelectManager.SeasonEnum name in System.Enum.GetValues(typeof(SelectManager.SeasonEnum)))
        {
            if (GUILayout.Toggle(selected == name, name.ToString(), "Radio"))
            {
                selected = name;
            }
        }

        // �ύX���������ꍇ�͕ۑ�
        if (selected != script.season)
        {
            Undo.RecordObject(script, "Change Option");
            script.season = selected;
            EditorUtility.SetDirty(script);
        }
    }
}