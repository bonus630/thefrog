using UnityEngine;
 using UnityEditor;


namespace br.corp.bonus630.unity
{

    [InitializeOnLoad]
    public static class MovePlayer
    {
        static MovePlayer()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;

            // Botão direito do mouse
            if (e.type == EventType.ContextClick)
            {
                // Prepara menu
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Custom/Player Here"), false, () =>
                {
                    // Pega o ponto no mundo a partir da posição do mouse
                    Vector2 mousePos = Event.current.mousePosition;
                    mousePos.y = sceneView.camera.pixelHeight - mousePos.y; // inverter Y
                    Vector3 worldPos = sceneView.camera.ScreenToWorldPoint(mousePos);

                    // Como é 2D, z = 0
                    worldPos.z = 0;

                    Undo.RecordObject(Selection.activeGameObject.transform, "Move Object");
                    GameObject.Find("Player").transform.position = worldPos;    
                    //Selection.activeGameObject.transform.position = worldPos;
                });

                menu.AddItem(new GUIContent("Custom/Log Position"), false, () =>
                {
                    Debug.Log("Mouse position: " + e.mousePosition);
                });

                // Mostra menu
                menu.ShowAsContext();

                e.Use(); // Marca o evento como usado
            }
        }
    }
}
