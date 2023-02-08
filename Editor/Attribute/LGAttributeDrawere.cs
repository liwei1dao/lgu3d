using System;
using UnityEditor;
using UnityEngine;
using lgu3d;

namespace lgu3d.Editor
{
    /// <summary>
    /// 自动以属性行为
    /// </summary>
    [CustomPropertyDrawer(typeof(LGAttributeRename))]
    public class LGAttributeDrawere : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            string name = ((LGAttributeRename)attribute).PropertyName;
            bool isWrite = ((LGAttributeRename)attribute).IsWrite;
            Rect LeftRect = new Rect(position.x, position.y, position.width / 2, position.height);
            Rect RightRect = new Rect(position.x + position.width / 2, position.y, position.width / 2, position.height);
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    EditorGUI.LabelField(LeftRect, name);
                    if (isWrite)
                        property.intValue = EditorGUI.IntField(RightRect, property.intValue);
                    else
                        EditorGUI.LabelField(RightRect, property.intValue.ToString());
                    return;
                case SerializedPropertyType.Float:
                    EditorGUI.LabelField(LeftRect, name);
                    if (isWrite)
                        property.floatValue = EditorGUI.FloatField(RightRect, property.floatValue);
                    else
                        EditorGUI.LabelField(RightRect, property.floatValue.ToString());
                    return;
                case SerializedPropertyType.Boolean:
                    EditorGUI.LabelField(LeftRect, name);
                    if (isWrite)
                        property.boolValue = EditorGUI.Toggle(RightRect, property.boolValue);
                    else
                        EditorGUI.LabelField(RightRect, property.boolValue.ToString());
                    return;
                case SerializedPropertyType.String:
                    EditorGUI.LabelField(LeftRect, name);
                    if (isWrite)
                        property.stringValue = EditorGUI.TextField(RightRect, property.stringValue);
                    else
                        EditorGUI.LabelField(RightRect, property.stringValue);
                    return;
                case SerializedPropertyType.Enum:
                    EditorGUI.LabelField(LeftRect, name);
                    if (isWrite)
                        property.enumValueIndex = EditorGUI.Popup(RightRect, property.enumValueIndex, property.enumNames);
                    else
                        EditorGUI.LabelField(RightRect, property.enumNames[property.enumValueIndex]);

                    return;
                case SerializedPropertyType.ObjectReference:
                    EditorGUI.LabelField(LeftRect, name);
                    if (isWrite)
                        property.objectReferenceValue = EditorGUI.ObjectField(RightRect, property.objectReferenceValue, typeof(Transform));
                    else
                        EditorGUI.LabelField(RightRect, property.objectReferenceValue == null ? "null" : property.objectReferenceValue.name);
                    return;
                default:
                    base.OnGUI(position, property, label);
                    return;
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
