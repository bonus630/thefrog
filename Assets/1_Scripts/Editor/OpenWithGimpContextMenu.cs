using UnityEngine;       using UnityEditor;
using System.Diagnostics;

namespace br.corp.bonus630.unity
{
    public class OpenWithGimpContextMenu 
    {
        [MenuItem("Assets/Open With GIMP", true)]
        private static bool ValidateOpenWithGimp()
        {
            // só habilita se for textura
            return Selection.activeObject is Texture2D;
        }

        [MenuItem("Assets/Open With GIMP")]
        private static void OpenSelectedWithGimp()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            string fullPath = System.IO.Path.GetFullPath(path);

            // Caminho do executável do GIMP (ajuste conforme instalação)
            string gimpPath = @"C:\Program Files\GIMP 3\bin\gimp-3.exe";

            Process.Start(gimpPath, "\"" + fullPath + "\"");
        }
    }

}

