using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;
namespace br.corp.bonus630.unity
{

    public class MethodRunnerWindow : EditorWindow
    {
        private GameObject targetGO;
        private MonoBehaviour[] scripts;
        private List<MethodInfo>[] methods;

        [MenuItem("Bonus630/Method Runner")]
        public static void ShowWindow()
        {
            GetWindow<MethodRunnerWindow>("Method Runner");
        }

        private void OnGUI()
        {
            GUILayout.Label("Selecione um GameObject", EditorStyles.boldLabel);
            targetGO = (GameObject)EditorGUILayout.ObjectField("GameObject", targetGO, typeof(GameObject), true);

            if (targetGO == null)
                return;

            // Buscar scripts e métodos
            if (GUILayout.Button("Carregar Scripts"))
            {
                LoadScriptsAndMethods();
            }

            if (scripts == null || methods == null)
                return;

            // Mostrar cada script e seus métodos
            for (int i = 0; i < scripts.Length; i++)
            {
                if (scripts[i] == null) continue;

                EditorGUILayout.Space();
                EditorGUILayout.LabelField(scripts[i].GetType().Name, EditorStyles.boldLabel);

                foreach (var method in methods[i])
                {
                    if (GUILayout.Button(method.Name))
                    {
                        InvokeMethod(scripts[i], method);
                    }
                }
            }
        }

        private void LoadScriptsAndMethods()
        {
            scripts = targetGO.GetComponents<MonoBehaviour>();
            methods = new List<MethodInfo>[scripts.Length];

            for (int i = 0; i < scripts.Length; i++)
            {
                var type = scripts[i].GetType();
                methods[i] = new List<MethodInfo>();

                foreach (var method in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly))
                {
                    // ignorar getters/setters ou métodos do MonoBehaviour base
                    if (method.IsSpecialName) continue;
                    methods[i].Add(method);
                }
            }
        }

        private void InvokeMethod(MonoBehaviour script, MethodInfo method)
        {
            try
            {
                method.Invoke(script, null);
                Debug.Log($"Método {method.Name} chamado em {script.name}");
            }
            catch (Exception e)
            {
                Debug.LogError($"Erro ao chamar {method.Name}: {e}");
            }
        }
    }

}
