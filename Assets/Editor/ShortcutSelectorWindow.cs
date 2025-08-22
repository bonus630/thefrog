using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace br.corp.bonus630.unity
{

    // Cria uma janela customizada no Editor chamada "Shortcut Selector"
    public class ShortcutSelectorWindow : EditorWindow
    {
        // Classe que representa cada atalho
        [System.Serializable]
        public class ShortcutEntry
        {
            public KeyCode key;             // Tecla de atalho
            public GameObject target;       // GameObject que será selecionado
            public bool listenNextKey = false; // Flag para capturar a próxima tecla pressionada
        }

        public List<ShortcutEntry> shortcuts = new List<ShortcutEntry>(); // Lista de atalhos

        // Cria o menu "Tools/Shortcut Selector" para abrir a janela
        [MenuItem("Bonus630/Shortcut Selector")]
        public static void OpenWindow()
        {
            GetWindow<ShortcutSelectorWindow>("Shortcut Selector"); // Abre a janela
        }

        // Função chamada pelo Unity para desenhar o GUI da janela
        private void OnGUI()
        {
            EditorGUILayout.LabelField("Atalhos para selecionar GameObjects", EditorStyles.boldLabel); // Título

            if (shortcuts == null) shortcuts = new List<ShortcutEntry>(); // Garante que a lista não seja nula

            Event e = Event.current; // Captura o evento atual do Editor (para detectar teclas)

            // Loop por cada atalho na lista
            for (int i = 0; i < shortcuts.Count; i++)
            {
                EditorGUILayout.BeginHorizontal(); // Começa uma linha horizontal no GUI

                // Botão para ativar "ouvir próxima tecla"
                if (GUILayout.Button(shortcuts[i].listenNextKey ? "Pressione tecla..." : "Ouvir tecla", GUILayout.Width(100)))
                {
                    shortcuts[i].listenNextKey = true; // Ativa flag para capturar a próxima tecla
                }

                // Se está ouvindo a próxima tecla
                if (shortcuts[i].listenNextKey && e.type == EventType.KeyDown)
                {
                    shortcuts[i].key = e.keyCode;  // Captura a tecla pressionada
                    shortcuts[i].listenNextKey = false; // Desativa a flag
                    e.Use(); // Consome o evento, evitando que ele se propague
                }
                else if (!shortcuts[i].listenNextKey)
                {
                    // EnumPopup padrão para selecionar a tecla manualmente
                    KeyCode oldKey = shortcuts[i].key; // Guarda a tecla antiga
                    shortcuts[i].key = (KeyCode)EditorGUILayout.EnumPopup(shortcuts[i].key, GUILayout.Width(100)); // Mostra dropdown

                    // Verifica se há conflito com outro atalho
                    string conflictMessage = GetConflictMessage(shortcuts[i], i);
                    if (!string.IsNullOrEmpty(conflictMessage))
                    {
                        shortcuts[i].key = oldKey; // Reverte para tecla anterior se houver conflito
                        GUIContent label = new GUIContent("⚠ Conflito!", conflictMessage); // Tooltip do conflito
                        GUILayout.Label(label, GUILayout.Width(120)); // Exibe o alerta com tooltip
                    }
                }

                // Campo para selecionar o GameObject associado ao atalho
                shortcuts[i].target = (GameObject)EditorGUILayout.ObjectField(shortcuts[i].target, typeof(GameObject), true);

                // Botão para remover o atalho da lista
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    shortcuts.RemoveAt(i); // Remove entrada
                }

                EditorGUILayout.EndHorizontal(); // Fecha a linha horizontal
            }

            // Botão para adicionar um novo atalho
            if (GUILayout.Button("Adicionar Atalho"))
            {
                shortcuts.Add(new ShortcutEntry()); // Adiciona nova entrada vazia
            }

            // Detecta pressionamento de tecla normalmente na janela
            if (e != null && e.type == EventType.KeyDown)
            {
                foreach (var sc in shortcuts)
                {
                    if (sc.key == e.keyCode && sc.target != null) // Se a tecla pressionada corresponde a algum atalho
                    {
                        Selection.activeGameObject = sc.target; // Seleciona o GameObject no Editor
                        e.Use(); // Consome o evento
                        break; // Sai do loop
                    }
                }
            }
        }

        // Função que verifica se um atalho está em conflito com outro
        private string GetConflictMessage(ShortcutEntry entry, int index)
        {
            if (entry.key == KeyCode.None) return null; // Nenhuma tecla, sem conflito

            for (int j = 0; j < shortcuts.Count; j++)
            {
                if (j != index && shortcuts[j].key == entry.key) // Verifica outro atalho com mesma tecla
                {
                    string targetName = shortcuts[j].target != null ? shortcuts[j].target.name : "sem GameObject"; // Nome do GameObject em conflito
                    return $"Conflito com: {targetName}"; // Retorna mensagem de conflito
                }
            }
            return null; // Sem conflito
        }
    }


}
