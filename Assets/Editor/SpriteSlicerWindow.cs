using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;
using UnityEditor.U2D.Sprites;
using System.Linq;



public class SpriteSlicerWindow : EditorWindow
{
    private Configs configs = new Configs();
    private Texture2D sprite;
    private static readonly string configFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SpriteSlicerWindowConfig.json");
    //internal static SpriteRect[] spriteRects;
    //internal static bool canGen = false;
    //private float tolerance = 0.1f;
    private Texture2D previewTexture;
    private List<ColorTolerance> colorsToRemove = new List<ColorTolerance>();
    private int slice = 0;
    private int directionY = -1;

    [MenuItem("Window/Custom Sprite Slicer")]
    public static void OpenWindow()
    {

        SpriteSlicerWindow window = GetWindow<SpriteSlicerWindow>();
        window.titleContent = new GUIContent("Custom Sprite Slicer by Bonus630");
        window.minSize = new Vector2(140, 320);

    }
    private void OnGUI()
    {
        //configs = Load();
        sprite = (Texture2D)EditorGUILayout.ObjectField("Sprite", sprite, typeof(Texture2D), false);
        configs.sliceName = EditorGUILayout.TextField("Name", configs.sliceName);
        configs.category = EditorGUILayout.TextField("Category", configs.category);
        EditorGUILayout.Space();
        // GUILayout.BeginHorizontal();
        //configs.slices = EditorGUILayout.IntField("Slices", configs.slices);
        configs.rows = EditorGUILayout.IntField("Rows", configs.rows);
        configs.cols = EditorGUILayout.IntField("Columns", configs.cols);
        // GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        configs.width = EditorGUILayout.IntField("Width", configs.width);
        configs.height = EditorGUILayout.IntField("Height", configs.height);
        EditorGUILayout.Space();
        configs.offsetX = EditorGUILayout.IntField("Offset X", configs.offsetX);
        configs.offsetY = EditorGUILayout.IntField("Offset Y", configs.offsetY);
        configs.paddingX = EditorGUILayout.IntField("Padding X", configs.paddingX);
        configs.paddingY = EditorGUILayout.IntField("Padding Y", configs.paddingY);
        EditorGUILayout.Space();
        if (GUILayout.Button("Generate", GUILayout.Height(20)))
        {
            if (sprite != null)
                Slice();
            else
                Debug.Log("Select a Sprite Sheet!");
        }
        EditorGUILayout.Space();
        Rect previewRect = GUILayoutUtility.GetRect(256, 256);
        if (sprite != null)
        {
            GeneratePreview();
            if (previewTexture != null)
            {
                GUI.DrawTexture(previewRect, previewTexture, ScaleMode.ScaleToFit);
                // GUILayout.BeginHorizontal();
                //GUILayout.EndHorizontal();
            }
            else
                slice = 0;
        }
        EditorGUILayout.LabelField(string.Format("Preview: {0}_{1}", configs.sliceName, slice));
        slice = EditorGUILayout.IntSlider(slice, 0, configs.rows * configs.cols - 1);
        EditorGUILayout.Space();
        GenerateColorsGUI();


        //Save(configs);
    }
    void GenerateColorsGUI()
    {
        GUILayout.Label("Select Colors to Remove", EditorStyles.boldLabel);

        for (int i = 0; i < colorsToRemove.Count; i++)
        {
            GUILayout.BeginHorizontal();
            colorsToRemove[i] = new ColorTolerance(EditorGUILayout.ColorField(colorsToRemove[i].color), colorsToRemove[i].tolerance);

            if (GUILayout.Button("X", GUILayout.Width(30)))
            {
                colorsToRemove.RemoveAt(i);
                GUILayout.EndHorizontal();
                return;
            }
            GUILayout.EndHorizontal();
            colorsToRemove[i] = new ColorTolerance(colorsToRemove[i].color, EditorGUILayout.Slider("Tolerance", colorsToRemove[i].tolerance, 0f, 1f));
            EditorGUILayout.Space();
        }

        if (GUILayout.Button("Add Color"))
        {
            colorsToRemove.Add(new ColorTolerance(Color.white, 0f));
        }
    }
    void GeneratePreview()
    {
        if (sprite == null) return;

        int cropX = Mathf.Clamp(configs.offsetX, 0, sprite.width - 1);
        int cropY = Mathf.Clamp(configs.offsetY, 0, sprite.height - 1);
        int cropWidth = Mathf.Clamp(configs.width, 1, sprite.width - cropX);
        int cropHeight = Mathf.Clamp(configs.height, 1, sprite.height - cropY);
        Color transparentColor = new Color(0, 0, 0, 0);
        previewTexture = new Texture2D(cropWidth, cropHeight);

        int x = configs.offsetX + ((slice % configs.cols) * (configs.width + configs.paddingX));
        int y = configs.offsetY + ((slice / configs.cols) * (configs.height + configs.paddingY)) * directionY;

        //Debug.Log("X:" + x + "Y:" + y);

        try
        {
            Color[] pixels = sprite.GetPixels(x, y, configs.width, configs.height);
            for (int j = 0; j < pixels.Length; j++)
            {
                foreach (var color in colorsToRemove)
                {
                    if (IsSimilar(pixels[j], color.color, color.tolerance))
                    {
                        pixels[j] = new Color(0, 0, 0, 0);
                    }
                }
            }
            previewTexture.SetPixels(pixels);
            previewTexture.Apply();
        }
        catch { }
    }

    private static Configs Load()
    {
        if (File.Exists(configFilePath))
        {
            string json = File.ReadAllText(configFilePath);
            return JsonUtility.FromJson<Configs>(configFilePath);
        }
        return null;
    }
    private static void Save(Configs configs)
    {
        string json = JsonUtility.ToJson(configs);
        File.WriteAllText(configFilePath, json);
    }
    private void PrepareRegion(string path)
    {
        //Unity use Y bottom - up
        int textureWidth = configs.cols * configs.width;
        int c = 1;

        Texture2D texture = new Texture2D(configs.cols * configs.rows * configs.width, configs.height);
        texture.filterMode = FilterMode.Point;
        int x = configs.offsetX;
        int y = configs.offsetY;

        for (int i = 0; i < configs.cols * configs.rows; i++)
        {
            Color[] pixels = null;
            try
            {
                pixels = sprite.GetPixels(x, y, configs.width, configs.height);
            }
            catch { Debug.Log("Please check yours sizes"); return; }
            for (int j = 0; j < pixels.Length; j++)
            {
                foreach (var color in colorsToRemove)
                {
                    if (IsSimilar(pixels[j], color.color, color.tolerance))
                    {
                        pixels[j] = new Color(0, 0, 0, 0);
                    }
                }
            }
            texture.SetPixels(configs.width * i, 0, configs.width, configs.height, pixels);
            x += configs.width + configs.paddingX;
            c++;
            if (c > configs.cols)
            {
                c = 1;
                x = configs.offsetX;
                y += (configs.height + configs.paddingY) * directionY;
            }

            texture.Apply();
            byte[] buffer = texture.EncodeToPNG();
            File.WriteAllBytes(path, buffer);
            AssetDatabase.Refresh();
        }
    }
    private bool IsSimilar(Color c1, Color c2, float tolerance)
    {
        return Mathf.Abs(c1.r - c2.r) <= tolerance &&
               Mathf.Abs(c1.g - c2.g) <= tolerance &&
               Mathf.Abs(c1.b - c2.b) <= tolerance;
    }
    private void Slice()
    {
        string path = AssetDatabase.GetAssetPath(sprite);
        string newPath = path.Substring(0, path.LastIndexOf('/') + 1);
        if (!string.IsNullOrEmpty(configs.category))
        {
            newPath += configs.category + '/';
            Directory.CreateDirectory(newPath);
        }
        newPath += configs.sliceName + ".png";

        if (File.Exists(newPath))
        {
            Debug.Log("File already exists!");
            //EditorUtility.DisplayDialog("Warning!", "File already exists!", "OK");
            return;

        }
        PrepareRegion(newPath);
        TextureImporter importer = TextureImporter.GetAtPath(newPath) as TextureImporter;

        if (importer == null)
            return;
        //var s = new SpriteSheetProcessor();
        //s.OnPreprocessTexture();
        //return;
        importer.isReadable = true;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
        importer.filterMode = FilterMode.Point;
        importer.spriteImportMode = SpriteImportMode.Multiple;



        int x = 0;

        //spriteRects = new SpriteRect[configs.cols * configs.rows];
        //for (int i = 0; i < configs.cols * configs.rows; i++)
        //{
        //    spriteRects[i] = new SpriteRect()
        //    {
        //        name = string.Format("{0}_{1}", configs.sliceName, i),
        //        spriteID = GUID.Generate(),
        //        rect = new Rect(x, 0, configs.width, configs.height),
        //        alignment = (int)SpriteAlignment.Center
        //    };
        //    x += configs.width;
        //}
        //canGen = true;
        SpriteMetaData[] metaDatas = new SpriteMetaData[configs.cols * configs.rows];
        for (int i = 0; i < configs.cols * configs.rows; i++)
        {
            metaDatas[i] = new SpriteMetaData
            {
                name = string.Format("{0}_{1}", configs.sliceName, i),
                rect = new Rect(x, 0, configs.width, configs.height),
                alignment = (int)SpriteAlignment.Center
            };
            x += configs.width;
            Debug.Log("x:" + x + "i:" + i);
        }

        importer.spritesheet = metaDatas;
        AssetDatabase.ImportAsset(newPath, ImportAssetOptions.ForceUpdate);


    }

}

//public class SpriteSheetProcessor : AssetPostprocessor
//{
//    public void OnPreprocessTexture()
//    {
//        if (!SpriteSlicerWindow.canGen)
//            return;
//        SpriteSlicerWindow.canGen = false;
//        var factory = new SpriteDataProviderFactories();
//        factory.Init();
//        var dataProvider = factory.GetSpriteEditorDataProviderFromObject(assetImporter);
//        dataProvider.InitSpriteEditorDataProvider();
//        SpriteRect[] rects = dataProvider.GetSpriteRects();
//        if (SpriteSlicerWindow.spriteRects != null)
//        {
//            //int rectsLenght = rects.Length;
//            //int totalLenght = rectsLenght + SpriteSlicerWindow.spriteRects.Length;

//            //Array.Resize(ref rects, totalLenght);
//            //for (int i = rectsLenght; i < totalLenght; i++)
//            //{
//            //    rects[i] = SpriteSlicerWindow.spriteRects[i - rectsLenght];
//            //}
//            //for (int i = 0; i < SpriteSlicerWindow.spriteRects.Length; i++)
//            //{
//            //    var s = new SpriteRect[1];
//            //    s[0] = SpriteSlicerWindow.spriteRects[i];
//            //    dataProvider.SetSpriteRects(s);
//            //}
//            dataProvider.SetSpriteRects(SpriteSlicerWindow.spriteRects);


//            var spriteNameFileIdDataProvider = dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
//            var nameFileIdPairs = spriteNameFileIdDataProvider.GetNameFileIdPairs().ToList();
//            for (int i = 0; i < nameFileIdPairs.Count; i++)
//            {
//                nameFileIdPairs.Add(new SpriteNameFileIdPair(nameFileIdPairs[i].name, nameFileIdPairs[i].GetFileGUID()));

//            }
//            spriteNameFileIdDataProvider.SetNameFileIdPairs(nameFileIdPairs);
//        }

//        dataProvider.Apply();
//        Debug.Log("AssetImporte:" + assetImporter.assetPath);
//    }
//}
    //static void AddSprite(ISpriteEditorDataProvider dataProvider, SpriteRect newSprite)
    //{

    //    var spriteRects = dataProvider.GetSpriteRects().ToList();
    //    spriteRects.Add(newSprite);

    //    // Write the updated data back to the data provider
    //    dataProvider.SetSpriteRects(spriteRects.ToArray());

    //    // Note: This section is only for Unity 2021.2 and newer
    //    // Register the new Sprite Rect's name and GUID with the ISpriteNameFileIdDataProvider
    //    var spriteNameFileIdDataProvider = dataProvider.GetDataProvider<ISpriteNameFileIdDataProvider>();
    //    var nameFileIdPairs = spriteNameFileIdDataProvider.GetNameFileIdPairs().ToList();
    //    nameFileIdPairs.Add(new SpriteNameFileIdPair(newSprite.name, newSprite.spriteID));
    //    spriteNameFileIdDataProvider.SetNameFileIdPairs(nameFileIdPairs);
    //    // End of Unity 2021.2 and newer section

    //    // Apply the changes
    //    dataProvider.Apply();
    //}
    //static void UpdateSettings()
    //{
    //    foreach (var obj in Selection.objects)
    //    {
    //        if (obj is Texture2D)
    //        {
    //            var factory = new SpriteDataProviderFactories();
    //            factory.Init();
    //            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(obj);
    //            dataProvider.InitSpriteEditorDataProvider();

    //            /* Use the data provider */

    //            // Apply the changes made to the data provider
    //            dataProvider.Apply();

    //            // Reimport the asset to have the changes applied
    //            var assetImporter = dataProvider.targetObject as AssetImporter;
    //            assetImporter.SaveAndReimport();
    //        }
    //    }
    //}
    //}
    [System.Serializable]
public class Configs
{
    // public Texture2D sprite { get; set; }
    public string sliceName { get; set; } = string.Empty;
    public string category { get; set; } = string.Empty;
    public int slices { get; set; } = 1;
    public int width { get; set; } = 48;
    public int height { get; set; } = 48;
    public int offsetX { get; set; } = 0;
    public int offsetY { get; set; } = 0;
    public int paddingX { get; set; } = 0;
    public int paddingY { get; set; } = 0;
    public int rows { get; set; } = 1;
    public int cols { get; set; } = 1;
}
public struct ColorTolerance
{
    public Color color;
    public float tolerance;

    public ColorTolerance(Color color, float tolerance)
    {
        this.color = color;
        this.tolerance = tolerance;
    }
}

