using Game.UI.Elements;
using System.Security;
using UnityEditor;
using UnityEditor.UI;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace Game.Editor
{
    [CustomEditor(typeof(NavigationButton))]
    public class NavigationButtonEditor : ButtonEditor
    {
        private SerializedProperty m_targetViewProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_targetViewProperty = serializedObject.FindProperty("m_targetView");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUILayout.PropertyField(m_targetViewProperty);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
