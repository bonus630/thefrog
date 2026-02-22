using UnityEngine;
using UnityEditor;
using System.Linq;
namespace br.corp.bonus630.unity
{
    [CustomEditor(typeof(br.com.bonus630.thefrog.Manager.SceneLoadManager))]
    public class SceneLoadManagerEditor : Editor
    {
        private string[] sceneNames;
        private int selectedIndex = 0;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            EditorGUILayout.LabelField("Editor ativo");

            var manager = (br.com.bonus630.thefrog.Manager.SceneLoadManager)target;
            manager.EnsureInitialized();

            // Pega os nomes das scenes do dicionário via reflection
            var field = typeof(br.com.bonus630.thefrog.Manager.SceneLoadManager)
                .GetField("sceneBlocks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var dict = field.GetValue(manager) as System.Collections.IDictionary;

            if (dict == null || dict.Count == 0)
                return;

            sceneNames = dict.Keys.Cast<string>().ToArray();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Selecionar Scene Blocks", EditorStyles.boldLabel);

            selectedIndex = EditorGUILayout.Popup("Scene", selectedIndex, sceneNames);

            if (GUILayout.Button("Selecionar Blocos"))
            {
                SelectBlocks(manager, sceneNames[selectedIndex]);
            }
        }

        private void SelectBlocks(br.com.bonus630.thefrog.Manager.SceneLoadManager manager, string sceneName)
        {
            var field = typeof(br.com.bonus630.thefrog.Manager.SceneLoadManager)
                .GetField("sceneBlocks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var dict = field.GetValue(manager) as System.Collections.IDictionary;

            if (!dict.Contains(sceneName))
                return;

            int[] blockIndexes = (int[])dict[sceneName];

            var blocksField = typeof(br.com.bonus630.thefrog.Manager.SceneLoadManager)
                .GetField("blocks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            var blocks = blocksField.GetValue(manager) as UnityEngine.Component[];

            Debug.LogWarning("[SceneLoadManagerEditor] blocks:" + ((Component)blocks[0]).ToString());
            foreach (int index in blockIndexes)
            {
                if (index >= 0 && index < blocks.Length && blocks[index] != null)
                {
                    blocks = blockIndexes
                        .Where(i => i >= 0 && i < blocks.Length)
                        .Select(i => blocks[i])
                        .ToArray();
                }
            }
            Selection.objects = blocks
                                .Select(c => c.gameObject)
                                .ToArray();
        }
    }

}

