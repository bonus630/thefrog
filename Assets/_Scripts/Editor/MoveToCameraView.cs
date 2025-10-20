using UnityEngine;
using UnityEditor;

namespace br.corp.bonus630.unity
{

public class MoveToCameraView
    {
        // Cria a opção no menu de contexto da Hierarchy (fica perto do "Select All")
        [MenuItem("GameObject/Move To Camera View %#m", false, 0)] // Ctrl+Shift+M de atalho
        static void MoveSelectedObjectsToCameraView()
        {
            if (Selection.transforms.Length == 0)
            {
                Debug.LogWarning("Nenhum objeto selecionado.");
                return;
            }

            SceneView sceneView = SceneView.lastActiveSceneView;
            if (sceneView == null || sceneView.camera == null)
            {
                Debug.LogWarning("Nenhuma câmera da SceneView encontrada.");
                return;
            }

            Camera cam = sceneView.camera;
            Undo.RecordObjects(Selection.transforms, "Move To Camera View");
            foreach (Transform t in Selection.transforms)
            {
                Vector3 targetPos = cam.transform.position + cam.transform.forward * 5f;
                t.position = targetPos;
            }
        }
        [MenuItem("GameObject/Distance between %#d", false, 0)] // Ctrl+Shift+d de atalho
        static void DebugDistance()
        {
            if (Selection.transforms.Length != 2)
            {
                Debug.LogWarning("Selecione dois objetos.");
                return;
            }
            Debug.Log($"Distância \"{Selection.gameObjects[0].name} e {Selection.gameObjects[1].name}\": " +
                $"{Vector3.Distance(Selection.gameObjects[0].transform.position,Selection.gameObjects[1].transform.position)}");
        }
    }


}
