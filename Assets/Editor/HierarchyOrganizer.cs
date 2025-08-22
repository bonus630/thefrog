using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;
namespace br.corp.bonus630.unity
{
    public class HierarchyOrganizer
    {
        [MenuItem("GameObject/Selection/Organizar Seleção por Nome", false, 20)]
        // [MenuItem("Selection/Organizar Seleção por Nome", false, 20)]
        static void OrganizeSelectedByName()
        {
            GameObject[] selectedObjects = Selection.gameObjects;

            if (selectedObjects.Length == 0)
            {
                Debug.LogWarning("Nenhum GameObject selecionado.");
                return;
            }

            // Ordena usando comparador customizado
            System.Array.Sort(selectedObjects, CompareByNameWithNumbers);

            // Reorganiza os objetos na hierarquia
            for (int i = 0; i < selectedObjects.Length; i++)
            {
                selectedObjects[i].transform.SetSiblingIndex(i);
            }

            Debug.Log("GameObjects organizados por nome (com suporte a números).");
        }

        [MenuItem("GameObject/Selection/Organizar Seleção por Nome", true)]
        // [MenuItem("Selection/Organizar Seleção por Nome", true)]
        static bool ValidateOrganizeSelectedByName()
        {
            return Selection.gameObjects.Length > 0;
        }

        // Comparador inteligente
        private static int CompareByNameWithNumbers(GameObject a, GameObject b)
        {
            // Regex para pegar nomes com número entre parênteses
            Regex regex = new Regex(@"^(.*?)(?: \((\d+)\))?$");

            Match matchA = regex.Match(a.name);
            Match matchB = regex.Match(b.name);

            string baseNameA = matchA.Groups[1].Value;
            string baseNameB = matchB.Groups[1].Value;

            int compareBase = string.Compare(baseNameA, baseNameB);
            if (compareBase != 0)
                return compareBase;

            // Se os nomes base forem iguais, comparar os números
            int numberA = matchA.Groups[2].Success ? int.Parse(matchA.Groups[2].Value) : 0;
            int numberB = matchB.Groups[2].Success ? int.Parse(matchB.Groups[2].Value) : 0;

            return numberA.CompareTo(numberB);
        }
    }
}
