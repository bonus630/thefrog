using UnityEngine;
using UnityEditor;
using System.IO;

namespace br.corp.bonus630.unity
{

    [InitializeOnLoad]
    public static class AutoRefreshWatcher
    {
        static FileSystemWatcher watcher;
        static bool pendingRefresh = false;

        static AutoRefreshWatcher()
        {
            SetupWatcher();

            EditorApplication.update += () =>
            {
                if (pendingRefresh)
                {
                    pendingRefresh = false;
                    AssetDatabase.Refresh();
                }
            };
        }

        static void SetupWatcher()
        {
            string assetsPath = Application.dataPath;

            watcher = new FileSystemWatcher(assetsPath, "*.cs");
            watcher.IncludeSubdirectories = true;
            watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size;

            watcher.Changed += (s, e) =>
            {
                pendingRefresh = true;
            };

            watcher.EnableRaisingEvents = true;

            Debug.Log("AutoReload: FileSystemWatcher iniciado.");
        }
    }

}
