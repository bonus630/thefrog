using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

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
            Undo.RecordObjects(Selection.transforms, "Move To Camera XY");
            foreach (Transform t in Selection.transforms)
            {
                Vector3 targetPos = cam.transform.position + cam.transform.forward * 5f;
                t.position = new Vector3(targetPos.x,targetPos.y,t.position.z);
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
        [MenuItem("GameObject/Names string join")]
        static void JoinNames()
        {
            if (Selection.transforms.Length == 0)
            {
                Debug.LogWarning("Nenhum objeto selecionado.");
                return;
            }
            string names = "";
            for(int i = 0; i < Selection.transforms.Length; i++)
            {
                int index = GetIndex(Selection.transforms[i].gameObject);
                if(index > -1)
                     names += index;
                if(i<Selection.transforms.Length-1)
                    names += ",";

            }
            GUIUtility.systemCopyBuffer = names;
            Debug.Log(names);
        }
        static int GetIndex(GameObject go)
        {
    
            if (go == null) return -1;

            // Procura o tipo pelo nome — sem referencia, sem namespace
            var type = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .FirstOrDefault(t => t.Name == "CollisionRelayEx");

            if (type == null)
            {
                Debug.LogError("Tipo CollisionRelayEx não encontrado em nenhum assembly.");
                return -1;
            }

            // Pega o componente pelo tipo encontrado
            var comp = go.GetComponent(type);
            if (comp == null)
            {
                Debug.LogError("O GameObject selecionado não possui CollisionRelayEx.");
                return -1;
            }

            // Seta a propriedade 'index' via reflexão
            var prop = type.GetField("index");
            if (prop == null)
            {
                Debug.LogError("O campo 'index' não foi encontrado no componente.");
                return -1;
            }
            return prop.GetValue(comp) is int index ? index : -1;
        }
    }


}
