using UnityEngine;
using UnityEditor;
using br.com.bonus630.thefrog.Environment;

namespace br.com.bonus630.thefrog
{
    [CustomEditor(typeof(ScenePointsData))]
    public class ScenePointsDataProcessor : Editor
    {
        SerializedProperty sceneIndexProp;
        SerializedProperty pointsDataProp;
        SerializedProperty sceneTypeProp;

        private void OnEnable()
        {
            sceneIndexProp = serializedObject.FindProperty("SceneIndex");
            pointsDataProp = serializedObject.FindProperty("PointsData");
            sceneTypeProp = serializedObject.FindProperty("SceneType");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(sceneIndexProp);

            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(sceneTypeProp);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Points", EditorStyles.boldLabel);

            for (int i = 0; i < pointsDataProp.arraySize; i++)
            {
                SerializedProperty element = pointsDataProp.GetArrayElementAtIndex(i);
                SerializedProperty nameProp = element.FindPropertyRelative("Name");
                SerializedProperty pointProp = element.FindPropertyRelative("Point");

                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();

                nameProp.stringValue = EditorGUILayout.TextField("Name", nameProp.stringValue);

                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    pointsDataProp.DeleteArrayElementAtIndex(i);
                    break;
                }

                EditorGUILayout.EndHorizontal();

                pointProp.vector3Value = EditorGUILayout.Vector3Field("Point", pointProp.vector3Value);

                // Campo temporário para arrastar Transform
                Transform transformInput = EditorGUILayout.ObjectField("Set From Transform", null, typeof(Transform), true) as Transform;
                if (transformInput != null)
                {
                    pointProp.vector3Value = transformInput.position;
                }

                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Add New Point"))
            {
                pointsDataProp.InsertArrayElementAtIndex(pointsDataProp.arraySize);
                SerializedProperty newElement = pointsDataProp.GetArrayElementAtIndex(pointsDataProp.arraySize - 1);
                newElement.FindPropertyRelative("Name").stringValue = "NewPoint";
                newElement.FindPropertyRelative("Point").vector3Value = Vector3.zero;
            }

            serializedObject.ApplyModifiedProperties();
        }

    }
}
