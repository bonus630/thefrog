using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

public class TeleporterRelinkerTool : EditorWindow
{
    [MenuItem("Tools/Teleporter Relinker Tool")]
    public static void ShowWindow()
    {
        GetWindow<TeleporterRelinkerTool>("Teleporter Relinker");
    }

    [System.Serializable]
    public class TeleporterRef
    {
        public string teleporterName;
        public string toName;
        public string fromName;
    }

    private List<TeleporterRef> references = new();

    void OnGUI()
    {
        if (GUILayout.Button("Extrair vínculos da cena atual"))
        {
            references = ExtrairReferencias();
            Debug.Log($"Extraídos {references.Count} vínculos.");
        }

        if (references.Count > 0 && GUILayout.Button("Aplicar vínculos na cena atual"))
        {
            AplicarReferencias();
            Debug.Log("Vínculos aplicados.");
        }
    }

    List<TeleporterRef> ExtrairReferencias()
    {
        var lista = new List<TeleporterRef>();
        var teleporters = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(mb => mb.GetType().Name == "Teleporter")
            .ToList();

        foreach (var mono in teleporters)
        {
            var tipo = mono.GetType();
            var teleportedGO = (GameObject)tipo.GetField("teleported", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(mono);
            var fromGO = (GameObject)tipo.GetField("from", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(mono);
            var toGO = (GameObject)tipo.GetField("to", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.GetValue(mono);

            lista.Add(new TeleporterRef
            {
                teleporterName = mono.name,
                fromName = fromGO ? fromGO.name : "",
                toName = toGO ? toGO.name : ""
            });
        }

        return lista;
    }

    void AplicarReferencias()
    {
        var teleporters = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None)
            .Where(mb => mb.GetType().Name == "Teleporter")
            .ToList();

        foreach (var item in references)
        {
            var teleporter = teleporters.FirstOrDefault(t => t.name == item.teleporterName);
            if (teleporter == null) continue;

            var tipo = teleporter.GetType();
            var allGOs = GameObject.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            var fromGO = allGOs.FirstOrDefault(go => go.name == item.fromName);
            var toGO = allGOs.FirstOrDefault(go => go.name == item.toName);

            tipo.GetField("from", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(teleporter, fromGO);
            tipo.GetField("to", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?.SetValue(teleporter, toGO);

            EditorUtility.SetDirty(teleporter);
        }

        AssetDatabase.SaveAssets();
    }
}
