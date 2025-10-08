using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Utils;
using System.Linq; // Namespace do seu SavesManager

namespace br.corp.bonus630.unity
{

    public class SaveEditorWindow : EditorWindow
    {
        private SaveStates save;
        private Vector2 scrollPos;
        private int saveIndex = 1;
        private SavesManager savesManager;
        private Dictionary<string, bool> foldouts = new Dictionary<string, bool>();

        [MenuItem("Bonus630/Save Editor")]
        public static void ShowWindow()
        {
            GetWindow<SaveEditorWindow>("Save Editor");
        }

        private void OnEnable()
        {
            savesManager = new SavesManager();
            LoadSave(saveIndex);
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            saveIndex = EditorGUILayout.IntSlider("Save Index", saveIndex, 1, 3);
            if (GUILayout.Button("Load Save"))
            {
                LoadSave(saveIndex);
            }
            if (GUILayout.Button("Save"))
            {
                SaveCurrent();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
       
            if (GUILayout.Button("Load From File"))
            {
                LoadFromFile();
            }
            if (GUILayout.Button("Save To File"))
            {
                SaveToFile();
            }
            EditorGUILayout.EndHorizontal();

            if (save == null)
            {
                EditorGUILayout.LabelField("Save não carregado.");
                return;
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

            EditorGUILayout.LabelField("Save Info", EditorStyles.boldLabel);
            save.index = EditorGUILayout.IntField("Index", save.index);
            save.thumb = EditorGUILayout.TextField("Thumbnail", save.thumb);

            if (save.environmentStates == null)
                save.environmentStates = new EnvironmentStates(new PlayerStates());

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Environment States", EditorStyles.boldLabel);

            var env = save.environmentStates;
            env.index = EditorGUILayout.IntField("Env Index", env.index);
            env.GameTimeInSeconds = EditorGUILayout.FloatField("Game Time (s)", env.GameTimeInSeconds);
            env.NPCVirtualGuyApples = EditorGUILayout.IntField("NPC Apples", env.NPCVirtualGuyApples);
            env.NPCVirtualGuyDialogue = EditorGUILayout.IntField("NPC Dialogue", env.NPCVirtualGuyDialogue);
            env.NPC_WallJump_Tutorial = EditorGUILayout.IntField("WallJump Tutorial", env.NPC_WallJump_Tutorial);

            DrawDatasList($"Activeds {env.Activeds.Count}", env.Activeds);

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Player States", EditorStyles.boldLabel);

            var player = env.playerStates;
            player.HasDoubleJump = EditorGUILayout.Toggle("Double Jump", player.HasDoubleJump);
            player.HasWallJump = EditorGUILayout.Toggle("Wall Jump", player.HasWallJump);
            player.HasDash = EditorGUILayout.Toggle("Dash", player.HasDash);
            player.HasGravity = EditorGUILayout.Toggle("Gravity", player.HasGravity);
            player.HasFireball = EditorGUILayout.Toggle("Fireball", player.HasFireball);
            player.HasLightning = EditorGUILayout.Toggle("Lightning", player.HasLightning);
            player.HasWind = EditorGUILayout.Toggle("Wind", player.HasWind);
            player.HasWater = EditorGUILayout.Toggle("Water", player.HasWater);
            player.HasEarth = EditorGUILayout.Toggle("Earth", player.HasEarth);
            player.FallsControl = EditorGUILayout.Toggle("Falls Control", player.FallsControl);

            player.Shurykens = EditorGUILayout.IntField("Shurykens", player.Shurykens);
            player.numDies = EditorGUILayout.IntField("Num Dies", player.numDies);
            player.MaxHearts = EditorGUILayout.IntField("Max Hearts", player.MaxHearts);
            player.Hearts = EditorGUILayout.IntField("Hearts", player.Hearts);
            player.Hour = EditorGUILayout.IntField("Hour", player.Hour);
            player.Collectables = EditorGUILayout.IntField("Collectables", player.Collectables);

            player.Speed = EditorGUILayout.FloatField("Speed", player.Speed);
            player.JumpForce = EditorGUILayout.FloatField("Jump Force", player.JumpForce);

            // PlayerPosition
            if (player.PlayerPosition == null)
                player.PlayerPosition = new PlayerPosition();
            EditorGUILayout.LabelField("Player Position", EditorStyles.boldLabel);
            player.PlayerPosition.CheckPointID = EditorGUILayout.IntField("CheckPointID", player.PlayerPosition.CheckPointID);
            player.PlayerPosition.Position = EditorGUILayout.Vector2Field("Position", player.PlayerPosition.Position);

            DrawDatasList($"CollectablesID {player.CollectablesID.Count}", player.CollectablesID);
            DrawDatasList($"ChestsID {player.ChestsID.Count}", player.ChestsID);
            DrawDatasList($"CompletedGameEvents {player.CompletedGameEvents.Count}", player.CompletedGameEvents);

            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            if (GUILayout.Button("Print Save To Console"))
            {
                Debug.Log(JsonUtility.ToJson(save, true));
            }
        }

        private void DrawDatasList(string label, Datas datas)
        {
            if (datas == null)
                datas = new Datas();

            // inicializa o foldout se ainda não existir
            if (!foldouts.ContainsKey(label))
                foldouts[label] = true;

            foldouts[label] = EditorGUILayout.Foldout(foldouts[label], label, true);
            if (!foldouts[label])
                return; // se estiver colapsado, não desenha nada

            int removeIndex = -1;
            for (int i = 0; i < datas.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                datas[i] = EditorGUILayout.TextField(datas[i]);
                if (GUILayout.Button("X", GUILayout.Width(20)))
                {
                    removeIndex = i;
                }
                EditorGUILayout.EndHorizontal();
            }

            if (removeIndex >= 0)
                datas.Remove(datas[removeIndex]);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add"))
                datas.Add("");
            EditorGUILayout.EndHorizontal();
        }


        private void LoadSave(int index)
        {
            save = savesManager.Load(index);
            if (save == null)
            {
                Debug.LogWarning($"Save {index} não encontrado. Criando novo.");
                save = new SaveStates(index, new EnvironmentStates(new PlayerStates()));
            }
        }

        private void SaveCurrent()
        {
            if (save == null) return;

            // Aqui você precisa passar a Camera do jogador para gerar thumbnail.  
            // Como estamos no editor, podemos passar null ou uma câmera de teste.
            savesManager.Save(save.index, save.environmentStates.playerStates, save.environmentStates, null);

            Debug.Log($"Save {save.index} salvo!");
        }
        private void LoadFromFile()
        {
            string path = EditorUtility.OpenFilePanel("Select Save File", Application.persistentDataPath, "dat");
            if (!string.IsNullOrEmpty(path))
            {
                string json = System.IO.File.ReadAllText(path);
                json = Cripter.Decrypt(json); // se o seu save estiver criptografado
                save = JsonUtility.FromJson<SaveStates>(json);
                Debug.Log("Save carregado de arquivo: " + path);
            }
        }

        private void SaveToFile()
        {
            string path = EditorUtility.SaveFilePanel("Save Save File", Application.persistentDataPath, $"TheFrogData{save.index}", "dat");
            if (!string.IsNullOrEmpty(path))
            {
                string json = JsonUtility.ToJson(save, true);
                json = Cripter.Encrypt(json); // se quiser manter criptografia
                System.IO.File.WriteAllText(path, json);
                Debug.Log("Save salvo em arquivo: " + path);
            }
        }

    }

}
