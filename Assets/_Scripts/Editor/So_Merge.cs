using UnityEngine;
using UnityEditor;
namespace br.corp.bonus630.unity
{
    public class SO_Merger
    {
        //[MenuItem("Tools/Merge ScriptableObjects")]
        //public static void Merge()
        //{
        //    string internPath = EditorUtility.OpenFilePanel("Selecione o asset INTERN", "Assets", "asset");
        //    if (string.IsNullOrEmpty(internPath)) return;

        //    string externPath = EditorUtility.OpenFilePanel("Selecione o asset EXTERN", "Assets", "asset");
        //    if (string.IsNullOrEmpty(externPath)) return;

        //    internPath = "Assets" + internPath.Replace(Application.dataPath, "");
        //    externPath = "Assets" + externPath.Replace(Application.dataPath, "");

        //    BaseData intern = AssetDatabase.LoadAssetAtPath<BaseData>(internPath);
        //    BaseData externData = AssetDatabase.LoadAssetAtPath<BaseData>(externPath);

        //    if (intern == null || externData == null)
        //    {
        //        Debug.LogError("Os dois assets precisam ser do mesmo tipo de ScriptableObject!");
        //        return;
        //    }

        //    // cria asset final
        //    string savePath = EditorUtility.SaveFilePanelInProject(
        //        "Salvar asset mesclado",
        //        "MergedData",
        //        "asset",
        //        "Escolha onde salvar"
        //    );

        //    if (string.IsNullOrEmpty(savePath)) return;

        //    BaseData merged = Object.Instantiate(intern); // copia base
        //    merged.areaType = AreaType.Intern; // marca origem intern

        //    // agora copia dados do extern *por cima*
        //    CopyValues(externData, merged);
        //    merged.areaType = AreaType.Extern; // marca origem extern

        //    AssetDatabase.CreateAsset(merged, savePath);
        //    AssetDatabase.SaveAssets();

        //    Debug.Log("Merge concluído e asset criado em:\n" + savePath);
        //}

        //private static void CopyValues(BaseData source, BaseData target)
        //{
        //    SerializedObject srcSO = new SerializedObject(source);
        //    SerializedObject dstSO = new SerializedObject(target);

        //    SerializedProperty prop = srcSO.GetIterator();

        //    if (prop.NextVisible(true))
        //    {
        //        do
        //        {
        //            if (prop.name == "m_Script") continue;
        //            if (prop.name == "areaType") continue; // não copiar, será definido manualmente

        //            dstSO.CopyFromSerializedProperty(prop);
        //        }
        //        while (prop.NextVisible(false));
        //    }

        //    dstSO.ApplyModifiedProperties();
        }
    }
