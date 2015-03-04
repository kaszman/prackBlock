using UniSave.Components;
using UnityEditor;
using UnityEngine;

namespace UniSave.Editor
{
    [InitializeOnLoad]
    public class HierarchyWindowIcon
    {
        private static Texture2D _texture;

        static HierarchyWindowIcon()
        {
            _texture = (Texture2D) Resources.Load("SaveableObject Icon");
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            var obj = (GameObject) EditorUtility.InstanceIDToObject(instanceID);

            if (obj != null && obj.GetComponent<SaveableObject>() != null)
            {
                var r = new Rect(selectionRect);
                r.x = r.xMax - 20;
                GUI.DrawTexture(new Rect(r.xMin, r.yMin + 2, 13, 13), _texture);
            }
        }
    }
}
