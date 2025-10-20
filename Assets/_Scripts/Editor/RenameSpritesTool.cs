using UnityEngine;
using UnityEditor;
using System.Linq;

namespace br.corp.bonus630.unity
{

    public class RenameSpritesTool : EditorWindow
    {
        private string prefix = "Alucard_walk_";
        private int startIndex = 0;
        private Object[] sprites;

        [MenuItem("Bonus630/Rename Sprites")]
        public static void ShowWindow()
        {
            GetWindow<RenameSpritesTool>("Rename Sprites");
        }

        private void OnGUI()
        {
            GUILayout.Label("Renomear Sprites", EditorStyles.boldLabel);
            prefix = EditorGUILayout.TextField("Prefixo", prefix);
            startIndex = EditorGUILayout.IntField("Índice inicial", startIndex);

            if (GUILayout.Button("Carregar seleção atual"))
            {
                sprites = Selection.objects.Where(o => o is Sprite).ToArray();
                Debug.Log($"Carregados {sprites.Length} sprites.");
            }

            if (sprites != null && sprites.Length > 0)
            {
                GUILayout.Label($"Sprites selecionados: {sprites.Length}");
                if (GUILayout.Button("Renomear"))
                {
                    RenameSelectedSprites();
                }
            }
        }

        private void RenameSelectedSprites()
        {
            if (sprites == null || sprites.Length == 0)
            {
                Debug.LogWarning("Nenhum sprite selecionado!");
                return;
            }

            // Ordena por nome, para manter sequência consistente
            var ordered = sprites.OrderBy(o => o.name).ToArray();

            string path = AssetDatabase.GetAssetPath(ordered[0]);
            var allAssets = AssetDatabase.LoadAllAssetsAtPath(path);

            int index = startIndex;
            foreach (var spriteObj in ordered)
            {
                string newName = $"{prefix}{index}";
                spriteObj.name = newName;
                EditorUtility.SetDirty(spriteObj);
                index++;
            }

            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"Renomeados {ordered.Length} sprites com prefixo '{prefix}'.");
        }
    }

}
