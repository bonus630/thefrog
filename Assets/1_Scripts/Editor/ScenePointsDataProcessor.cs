using UnityEngine;
using UnityEditor;
using br.com.bonus630.thefrog.Environment;
using br.com.bonus630.thefrog.Manager;

namespace br.corp.bonus630.unity
{
    [CustomEditor(typeof(ScenePointsData))]
    public class ScenePointsDataProcessor : Editor
    {
        private int activeIndex = -1;


        SerializedProperty sceneIndexProp;
        SerializedProperty pointsDataProp;
        SerializedProperty sceneTypeProp;


        private void OnEnable()
        {
            sceneIndexProp = serializedObject.FindProperty("SceneIndex");
            pointsDataProp = serializedObject.FindProperty("PointsData");
            sceneTypeProp = serializedObject.FindProperty("SceneType");
            SceneView.duringSceneGui += SceneView_duringSceneGui;
        }
        private void OnDisable()
        {
            activeIndex = -1;
            SceneView.RepaintAll();
            SceneView.duringSceneGui -= SceneView_duringSceneGui;
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
                EditorGUILayout.BeginHorizontal();
                pointProp.vector3Value = EditorGUILayout.Vector3Field("Point", pointProp.vector3Value);
                if (GUILayout.Button("P", GUILayout.Width(20)))
                {
                    activeIndex = i;
                    SceneView.RepaintAll();
                }
                EditorGUILayout.EndHorizontal();
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
                EditorGUILayout.Space();
            }
            EditorGUILayout.Space();
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
        private void SceneView_duringSceneGui(SceneView obj)
        {
            if (activeIndex < 0) return;
            if (activeIndex >= pointsDataProp.arraySize) return;
            var data = pointsDataProp.GetArrayElementAtIndex(activeIndex);
            var point = data.FindPropertyRelative("Point").vector3Value;
            Handles.color = Color.yellow;
            Handles.DrawWireDisc(point, Vector3.forward, 4f);
            // opcional: ponto central
            Handles.DotHandleCap(0, point, Quaternion.identity, 0.1f, EventType.Repaint);
            Handles.DrawLine(point - Vector3.right * 5, point + Vector3.right *5);
            Handles.DrawLine(point - Vector3.up * 5, point + Vector3.up * 5);
        }
    }
}
