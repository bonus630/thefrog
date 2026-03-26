using UnityEngine;
using UnityEditor;
using br.com.bonus630.thefrog.Environment;
using br.com.bonus630.thefrog.Manager;

namespace br.corp.bonus630.unity
{
    [CustomEditor(typeof(SceneMover))]
    public class ScenePointDropDown : Editor
    {
        SerializedProperty dataAssetProp;
        SerializedProperty selectedIndexProp;
        SerializedProperty useCurrentHourProp;

        void OnEnable()
        {
            dataAssetProp = serializedObject.FindProperty("scenePointsData");
            selectedIndexProp = serializedObject.FindProperty("ToPoint");
            useCurrentHourProp = serializedObject.FindProperty("useCurrentHour");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(dataAssetProp);

            ScenePointsData dataAsset = dataAssetProp.objectReferenceValue as ScenePointsData;

            if (dataAsset != null && dataAsset.PointsData != null && dataAsset.PointsData.Count > 0)
            {
                //string[] options = new string[dataAsset.PointsData.Count];
                //for (int i = 0; i < options.Length; i++)
                //{
                //    options[i] = dataAsset.PointsData[i].Name;
                //}

                //selectedIndexProp.intValue = EditorGUILayout.Popup("Selected Point", selectedIndexProp.intValue, options);
                GUIContent[] options = new GUIContent[dataAsset.PointsData.Count];
                for (int i = 0; i < options.Length; i++)
                {
                    string name = dataAsset.PointsData[i].Name;
                    string alias = dataAsset.PointsData[i].GameObjectName; // supondo que tenha Alias no seu objeto
                    options[i] = new GUIContent(name, alias);
                }

                selectedIndexProp.intValue = EditorGUILayout.Popup(
                    new GUIContent("Selected Point", "Escolha um ponto da lista."),
                    selectedIndexProp.intValue,
                    options
                );
                // Exibe posição do ponto selecionado (somente leitura)
                Vector3 selectedPoint = dataAsset.PointsData[selectedIndexProp.intValue].Point;
                EditorGUILayout.Vector3Field("Selected Point Position", selectedPoint);
            }
            else
            {
                EditorGUILayout.HelpBox("Atribua um ScenePointsData com pelo menos um item.", MessageType.Info);
            }
            EditorGUILayout.PropertyField(useCurrentHourProp);
            serializedObject.ApplyModifiedProperties();
        }

    }
}

