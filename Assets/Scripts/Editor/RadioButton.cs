using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// 汎用的なラジオボタンEditor。Enum型のフィールドを自動でラジオボタン化する。
/// </summary>
[CustomEditor(typeof(GoalScript), true)]
public class GenericRadioButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // インスペクターの最新状態を取得

        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;

        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;

            // Enum型のフィールドをラジオボタン化
            if (prop.propertyType == SerializedPropertyType.Enum)
            {
                DrawEnumAsRadioButtons(prop);
            }
            else
            {
                EditorGUILayout.PropertyField(prop, true);
            }
        }

        serializedObject.ApplyModifiedProperties(); // 変更を適用
    }

    /// <summary>
    /// Enum型のプロパティをラジオボタンとして表示する
    /// </summary>
    private void DrawEnumAsRadioButtons(SerializedProperty prop)
    {
        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(prop.name)); // フィールド名を表示

        EditorGUI.BeginChangeCheck();
        int selectedIndex = prop.enumValueIndex;
        string[] enumNames = prop.enumNames;

        for (int i = 0; i < enumNames.Length; i++)
        {
            if (GUILayout.Toggle(selectedIndex == i, enumNames[i], "Radio"))
            {
                selectedIndex = i;
            }
        }

        if (EditorGUI.EndChangeCheck())
        {
            prop.enumValueIndex = selectedIndex;
        }
    }
}
