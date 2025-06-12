using UnityEngine;
using UnityEditor;
using br.com.bonus630.thefrog.Environment;

namespace br.com.bonus630.thefrog
{
    [CustomEditor(typeof(SceneMover))]
    public class ScenePointDropDown : Editor
    {
        SerializedProperty dataAssetProp;
        SerializedProperty selectedIndexProp;

        void OnEnable()
        {
            dataAssetProp = serializedObject.FindProperty("scenePointsData");
            selectedIndexProp = serializedObject.FindProperty("ToPoint");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(dataAssetProp);

            ScenePointsData dataAsset = dataAssetProp.objectReferenceValue as ScenePointsData;

            if (dataAsset != null && dataAsset.PointsData != null && dataAsset.PointsData.Count > 0)
            {
                string[] options = new string[dataAsset.PointsData.Count];
                for (int i = 0; i < options.Length; i++)
                {
                    options[i] = dataAsset.PointsData[i].Name;
                }

                selectedIndexProp.intValue = EditorGUILayout.Popup("Selected Point", selectedIndexProp.intValue, options);

                // Exibe posição do ponto selecionado (somente leitura)
                Vector3 selectedPoint = dataAsset.PointsData[selectedIndexProp.intValue].Point;
                EditorGUILayout.Vector3Field("Selected Point Position", selectedPoint);
            }
            else
            {
                EditorGUILayout.HelpBox("Atribua um ScenePointsData com pelo menos um item.", MessageType.Info);
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}

