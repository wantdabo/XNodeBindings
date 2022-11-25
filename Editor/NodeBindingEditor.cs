using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace XNodeBinding
{
    [CustomEditor(typeof(NodeBinding))]
    public class NodeBindingEditor : Editor
    {
        private ReorderableList reorderableList;

        private void OnEnable()
        {
            if (null == serializedObject) return;

            reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("nodeList"), true, true, true, true);

            reorderableList.drawHeaderCallback = (Rect rect) => GUI.Label(rect, "NodeBindings");
            reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                SerializedProperty item = reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                EditorGUI.PropertyField(rect, item);
            };
        }

        public override void OnInspectorGUI()
        {
            if (null == reorderableList) return;
            serializedObject.Update();
            reorderableList.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }
    }
}
