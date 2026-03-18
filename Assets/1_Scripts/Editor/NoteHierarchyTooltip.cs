using UnityEngine;
using UnityEditor;
using br.com.bonus630.thefrog.Manager;

namespace br.corp.bonus630.unity
{
    [InitializeOnLoad]
    public static class NoteHierarchyTooltip
    {
        static NoteHierarchyTooltip()
        {
            // Hook para desenhar na hierarquia
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (obj == null) return;

            Note note = obj.GetComponent<Note>();
            if (note != null && !string.IsNullOrEmpty(note.comment))
            {
                EditorGUI.DrawRect(selectionRect, note.noteColor *0.3f); 
                // cor translúcida
                // Estilo customizado
                GUIStyle style = new GUIStyle(EditorStyles.label)
                {
                    fontStyle = FontStyle.Bold,
                    normal = { textColor = Color.red },
                    alignment = TextAnchor.MiddleLeft
                };
                // Verifica se o mouse está sobre o item da hierarquia

                // Desenhar um ícone **antes do nome**
                //Rect iconRect = new Rect(selectionRect.x, selectionRect.y, 16, selectionRect.height);
                //EditorGUI.LabelField(iconRect, "📝"); // ou usar Texture2D

                //// Ajustar o rect do nome para não sobrepor o ícone
                //Rect nameRect = new Rect(selectionRect.x + 18, selectionRect.y, selectionRect.width - 18, selectionRect.height);

                //// Tooltip
                //GUI.Label(nameRect, new GUIContent(obj.name, note.comment));
               


                if (selectionRect.Contains(Event.current.mousePosition))
                {
                    GUI.Label(selectionRect, new GUIContent("", note.comment),style);
                    // O tooltip será mostrado automaticamente
                }
            }
        }
    }
    
}
