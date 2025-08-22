using UnityEngine;
using UnityEditor;
using br.com.bonus630.thefrog.Environment;
using br.com.bonus630.thefrog.Manager;

namespace br.corp.bonus630.unity
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
                SerializedProperty gameObjectNameProp = element.FindPropertyRelative("GameObjectName");
                SerializedProperty hour = element.FindPropertyRelative("Hour");
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
                EditorGUILayout.BeginHorizontal();
                gameObjectNameProp.stringValue = EditorGUILayout.TextField("GameObjectName", gameObjectNameProp.stringValue);
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                hour.intValue = EditorGUILayout.IntField("Hour", hour.intValue);
                EditorGUILayout.EndHorizontal();
                pointProp.vector3Value = EditorGUILayout.Vector3Field("Point", pointProp.vector3Value);

                // Campo temporário para arrastar Transform
               // Transform transformInput = EditorGUILayout.ObjectField("Set From Transform", null, typeof(Transform), true) as Transform;
                //if (transformInput != null)
                //{
                //    pointProp.vector3Value = transformInput.position;
                //}
                GameObject gameObjectInput = EditorGUILayout.ObjectField("Set From GameObject", null, typeof(GameObject), true) as GameObject;
                if (gameObjectInput != null)
                {
                    Vector3 pos = gameObjectInput.transform.position;

                    // Trunca para duas casas decimais
                    pos.x = Mathf.Round(pos.x * 100f) / 100f;
                    pos.y = Mathf.Round(pos.y * 100f) / 100f;
                    pos.z = 0;

                    pointProp.vector3Value = pos;               // Atribui a posição truncada
                    gameObjectNameProp.stringValue = gameObjectInput.name;
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
