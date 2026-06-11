#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Util.Editor
{
    [CustomEditor(typeof(MonoBinder), true)]
    public class MonoBinderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            MonoBinder binder = (MonoBinder)target;

            if (GUILayout.Button("Bind"))
            {
                binder.Bind();
                EditorUtility.SetDirty(binder);
            }
            
            GUILayout.Space(10);
            
            DrawDefaultInspector();
        }
    }
}

#endif