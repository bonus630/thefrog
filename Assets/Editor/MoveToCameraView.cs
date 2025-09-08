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
                // pega a posição central da câmera (um pouco à frente do plano near)
                Vector3 targetPos = cam.transform.position + cam.transform.forward * 5f;
                t.position = targetPos;
            }
        }
    }


}
