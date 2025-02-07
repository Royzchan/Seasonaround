using UnityEditor;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// �ėp�I�ȃ��W�I�{�^��Editor�BEnum�^�̃t�B�[���h�������Ń��W�I�{�^��������B
/// </summary>
[CustomEditor(typeof(GoalScript), true)]
public class GenericRadioButtonEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // �C���X�y�N�^�[�̍ŐV��Ԃ��擾

        SerializedProperty prop = serializedObject.GetIterator();
        bool enterChildren = true;

        while (prop.NextVisible(enterChildren))
        {
            enterChildren = false;

            // Enum�^�̃t�B�[���h�����W�I�{�^����
            if (prop.propertyType == SerializedPropertyType.Enum)
            {
                DrawEnumAsRadioButtons(prop);
            }
            else
            {
                EditorGUILayout.PropertyField(prop, true);
            }
        }

        serializedObject.ApplyModifiedProperties(); // �ύX��K�p
    }

    /// <summary>
    /// Enum�^�̃v���p�e�B�����W�I�{�^���Ƃ��ĕ\������
    /// </summary>
    private void DrawEnumAsRadioButtons(SerializedProperty prop)
    {
        EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(prop.name)); // �t�B�[���h����\��

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
