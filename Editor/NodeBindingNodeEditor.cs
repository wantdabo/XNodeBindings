using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace XNodeBinding
{
    [CustomPropertyDrawer(typeof(NodeBinding.Node))]
    public class NodeBindingNodeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(rect, label, property))
            {
                EditorGUIUtility.labelWidth = 60;
                rect.height = EditorGUIUtility.singleLineHeight;

                float offset = 20 / rect.width;
                var name = property.FindPropertyRelative("name");
                var type = property.FindPropertyRelative("type");
                var gameObjectProperty = property.FindPropertyRelative("gameObject");
                var gameObject = gameObjectProperty.objectReferenceValue as GameObject;

                Rect nameRect = new Rect(rect)
                {
                    width = rect.width * 0.4f
                };

                Rect nodeRect = new Rect(nameRect)
                {
                    x = nameRect.x + nameRect.width + 10,
                    width = rect.width * (null == gameObject ? 0.6f - offset / 2 : (0.4f - offset)),
                };

                Rect typeRect = new Rect(nodeRect)
                {
                    x = nodeRect.x + nodeRect.width + 10,
                    width = rect.width * 0.2f,
                };


                name.stringValue = EditorGUI.TextField(nameRect, name.stringValue);

                NodeBinding.NodeType enumType = null == gameObject ? NodeBinding.NodeType.GameObject : (NodeBinding.NodeType)type.enumValueIndex;


                if (null != gameObject)
                {
                    enumType = (NodeBinding.NodeType)EditorGUI.EnumPopup(typeRect, enumType);
                    if (null == gameObjectProperty.objectReferenceValue) enumType = NodeBinding.NodeType.GameObject;
                    type.enumValueIndex = (int)enumType;
                }

                if (enumType == NodeBinding.NodeType.Component)
                {
                    var nodeComps = gameObject.GetComponents<Component>();

                    var compProperty = property.FindPropertyRelative("component");
                    if (null == compProperty.objectReferenceValue) compProperty.objectReferenceValue = nodeComps[0];
                    var curSelComp = compProperty.objectReferenceValue as Component;

                    string curSelCompName = $"{curSelComp.GetType()}({curSelComp.GetInstanceID()})";
                    string[] nodeCompNames = new string[nodeComps.Length];

                    int curSelIndex = 0;
                    for (int i = 0; i < nodeComps.Length; i++)
                    {
                        nodeCompNames[i] = $"{nodeComps[i].GetType()}({nodeComps[i].GetInstanceID()})";
                        if (curSelCompName.Equals(nodeCompNames[i])) curSelIndex = i;
                    }
                    var newSelIdx = EditorGUI.Popup(nodeRect, curSelIndex, nodeCompNames);
                    compProperty.objectReferenceValue = nodeComps[newSelIdx];
                }
                else
                {
                    gameObjectProperty.objectReferenceValue = EditorGUI.ObjectField(nodeRect, gameObjectProperty.objectReferenceValue, typeof(GameObject), true);
                }
            }
        }
    }
}
