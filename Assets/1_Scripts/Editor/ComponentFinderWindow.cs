using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace br.corp.bonus630.unity
{

    public class ComponentFinderWindow : EditorWindow
    {
        private Type selectedType;
        private Vector2 scroll;

        [MenuItem("Bonus630/Component Finder")]
        public static void OpenWindow()
        {
            GetWindow<ComponentFinderWindow>("Component Finder");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Select a Component Type", EditorStyles.boldLabel);

            // Dropdown com tipos comuns da Unity
            if (GUILayout.Button(selectedType != null ? selectedType.Name : "Select Component Type"))
            {
                GenericMenu menu = new GenericMenu();

                // Adiciona alguns tipos (pode expandir)
                AddTypeToMenu<AudioSource>(menu);
                AddTypeToMenu<Camera>(menu);
                AddTypeToMenu<Light>(menu);
                AddTypeToMenu<MeshRenderer>(menu);
                AddTypeToMenu<Rigidbody>(menu);

                menu.ShowAsContext();
            }

            if (selectedType == null)
                return;

            if (GUILayout.Button("Search"))
            {
                FindComponents();
            }

            GUILayout.Space(10);
            scroll = GUILayout.BeginScrollView(scroll);

            if (results != null)
            {
                foreach (var entry in results)
                {
                    EditorGUILayout.LabelField(entry, EditorStyles.label);
                }
            }

            GUILayout.EndScrollView();
        }

        private void AddTypeToMenu<T>(GenericMenu menu)
        {
            menu.AddItem(new GUIContent(typeof(T).Name), false, () => selectedType = typeof(T));
        }

        private List<string> results;

        private void FindComponents()
        {
            results = new List<string>();

            UnityEngine.Object[] objs = FindObjectsOfType(selectedType);
            foreach (var obj in objs)
            {
                Component comp = obj as Component;
                if (comp != null)
                {
                    string path = GetHierarchyPath(comp.gameObject);
                    results.Add(path);
                }
            }
        }

        private string GetHierarchyPath(GameObject go)
        {
            string path = go.name;
            Transform parent = go.transform.parent;
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            return path;
        }
    }

}
