using UnityEngine;
using UnityEditor;

namespace br.corp.bonus630.unity
{
    [InitializeOnLoad] // garante que ativa junto com o Editor
    public static class ColliderVisualizerToggle2
    {
        private static bool enabled = false;

        static ColliderVisualizerToggle2()
        {
            SceneView.duringSceneGui += OnSceneGUI;
        }

        [MenuItem("Bonus630/Collider Visualizer/Toggle %#e")] // Ctrl+Shift+E para ativar/desativar
        private static void Toggle()
        {
            enabled = !enabled;
            SceneView.RepaintAll(); // força atualizar a cena
        }

        [MenuItem("Bonus630/Collider Visualizer/Toggle %#e", true)]
        private static bool ToggleValidate()
        {
            Menu.SetChecked("Bonus630/Collider Visualizer/Toggle %#e", enabled);
            return true;
        }

        private static void OnSceneGUI(SceneView sceneView)
        {
            if (!enabled) return;

            Handles.color = Color.green;

            Collider2D[] colliders = Object.FindObjectsByType<Collider2D>(FindObjectsSortMode.None);
            foreach (var col in colliders)
            {
                if (col is BoxCollider2D box)
                {
                    Vector3 pos = box.transform.position + (Vector3)box.offset;
                    Handles.DrawWireCube(pos, box.size);
                }
                else if (col is CircleCollider2D circle)
                {
                    Vector3 pos = circle.transform.position + (Vector3)circle.offset;
                    Handles.DrawWireDisc(pos, Vector3.back, circle.radius);
                }
                else if (col is PolygonCollider2D poly)
                {
                    Vector3 pos = poly.transform.position;
                    var points = poly.points;
                    for (int i = 0; i < points.Length; i++)
                    {
                        Vector3 p1 = pos + (Vector3)points[i];
                        Vector3 p2 = pos + (Vector3)points[(i + 1) % points.Length];
                        Handles.DrawLine(p1, p2);
                    }
                }
            }
        }
    }
}

