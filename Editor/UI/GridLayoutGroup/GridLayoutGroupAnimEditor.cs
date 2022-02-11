using UnityEditor;
using UnityEditor.UI;

namespace lgu3d.Editor
{
    [CustomEditor(typeof(GridLayoutGroupAnim), true)]
    [CanEditMultipleObjects]
    public class GridLayoutGroupAnimEditor  : GridLayoutGroupEditor
    {
        SerializedProperty m_animTime;
        protected override void OnEnable()
        {
            base.OnEnable();
            m_animTime = serializedObject.FindProperty("m_animTime");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(m_animTime, true);
            base.OnInspectorGUI();
        }
    }
}