using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using br.com.bonus630.thefrog.Utils;

namespace br.com.bonus630.thefrog.Manager
{
    public class SavesManager
    {
        private readonly string fileName = "TheFrogData";
        //  public static SavesManager Instance;
        //public string SaveDataFilePath { get; private set; }
        public bool CanContinue()
        {
#if UNITY_WEBGL
            return PlayerPrefs.HasKey(FileName(0));

#else
            return File.Exists(FilePath(0));
#endif
        }
        public SavesManager()
        {
            //if(SavesManager.Instance==null)
            //    SavesManager.Instance = new SavesManager(); 
        }

        public bool Save(int index, PlayerStates playerStates, EnvironmentStates environmentStates, Camera camera)
        {

            try
            {
                environmentStates.playerStates = playerStates;
                SaveStates saveStates = new SaveStates(index, environmentStates);

                if (index > 0)
                {
                    Utils.ThumbGenerator thumb = new Utils.ThumbGenerator(0.1f);
                    saveStates.thumb = thumb.CreateEncodeThumb(camera, GameManager.Instance.GetPlayer);
                }

                string json = JsonUtility.ToJson(saveStates);
                json = Cripter.Encrypt(json);

#if UNITY_WEBGL
        PlayerPrefs.SetString(FileName(index), json);
        PlayerPrefs.Save();

        // PlayerPrefs não lança erro, então checamos se foi salvo mesmo
        return PlayerPrefs.GetString(FileName(index)) == json;
#else
                File.WriteAllText(FilePath(index), json);
                return File.Exists(FilePath(index)); // Checa se o arquivo realmente foi gravado
#endif
            }
            catch (Exception e)
            {
                Debug.LogError($"Erro ao salvar: {e}");
                return false;
            }
        }
        private string FileName(int index) => $"{fileName}{index}";
        private string FilePath(int index)
        {
#if UNITY_EDITOR
            return Path.Combine(Application.persistentDataPath, $"{FileName(index)}-editor.dat");
#else
            return Path.Combine(Application.persistentDataPath, $"{FileName(index)}.dat");
#endif
        }

        public SaveStates Load(int index)
        {
#if UNITY_WEBGL
            if (PlayerPrefs.HasKey(FileName(index)))
            {
                string json = PlayerPrefs.GetString(FileName(index), string.Empty);
                return JsonUtility.FromJson<SaveStates>(json);
            }

#else
            if (File.Exists(FilePath(index)))
            {
                string json = File.ReadAllText(FilePath(index));
                json = Cripter.Decrypt(json);
                return JsonUtility.FromJson<SaveStates>(json);
            }
#endif
            return null;
        }

        public List<SaveStates> ListSaves()
        {
            List<SaveStates> list = new List<SaveStates>();

            list.Add(Load(1));
            list.Add(Load(2));
            list.Add(Load(3));

            return list;
        }
    }
}
