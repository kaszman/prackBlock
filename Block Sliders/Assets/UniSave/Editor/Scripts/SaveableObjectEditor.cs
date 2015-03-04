using System;
using UniSave.Components;
using UnityEngine;
using UnityEditor;

namespace UniSave.Editor
{
    [CanEditMultipleObjects, CustomEditor(typeof (SaveableObject))]
    internal sealed class SaveableObjectEditor : UnityEditor.Editor
    {
        private SaveableObject _saveableObject;
        private SerializedProperty _ignoreSaveScene;
        private SerializedProperty _names;
        private SerializedProperty _toggles;

        public void OnEnable()
        {
            _saveableObject = (SaveableObject) target;
            _saveableObject.DetectComponents();

            _ignoreSaveScene = serializedObject.FindProperty("_ignoreSaveScene");
            _names = serializedObject.FindProperty("_componentNames");
            _toggles = serializedObject.FindProperty("_componentToggles");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space();
            _ignoreSaveScene.boolValue = EditorGUILayout.Toggle("Ignore SaveScene()", _ignoreSaveScene.boolValue);
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical("Box");
            EditorGUILayout.LabelField("Classes", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Toggle("GameObject", true);
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Components", EditorStyles.boldLabel);

            for(int i = 0; i < _toggles.arraySize; i++)
            {
                var componentName = _names.GetArrayElementAtIndex(i).stringValue;

                if (componentName == "SaveableObject")
                {
                    continue;
                }

                _toggles.GetArrayElementAtIndex(i).boolValue = EditorGUILayout.Toggle(componentName, _toggles.GetArrayElementAtIndex(i).boolValue);
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.Toggle("Instantiate", _saveableObject.WasInstantiated);
            EditorGUILayout.Toggle("Destroy", !_saveableObject.WasInstantiated);
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndVertical();

            if (_saveableObject.Id != Guid.Empty.ToString())
            {
                EditorGUILayout.Space();
                EditorGUILayout.PrefixLabel("ID");
                EditorGUILayout.BeginHorizontal("TextField");
                EditorGUILayout.SelectableLabel(_saveableObject.Id, GUILayout.Height(15));
                EditorGUILayout.EndHorizontal();
            }

            if (_saveableObject.transform.parent != null && _saveableObject.transform.parent.GetComponent<SaveableObject>() == null)
            {
                _saveableObject.transform.parent.gameObject.AddComponent<SaveableObject>();
                //Debug.Log("Added SaveableObject to parent.");
            }

            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }
    }
}
