using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;

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

        public void Save(int index, PlayerStates playerStates, EnvironmentStates environmentStates,Camera camera)
        {
            //Debug.Log("game time: " + environmentStates.GameTimeInSeconds);
            environmentStates.playerStates = playerStates;
            SaveStates saveStates = new SaveStates(index, environmentStates);
            if (index > 0)
            {
                Utils.ThumbGenerator thumb = new Utils.ThumbGenerator();
                saveStates.thumb = thumb.CreateEncodeThumb(camera, GameManager.Instance.GetPlayer, 0.1f);
            }
            string jason = JsonUtility.ToJson(saveStates);
#if UNITY_WEBGL
            PlayerPrefs.SetString(FileName(index), jason);
            PlayerPrefs.Save();
#else
            File.WriteAllText(FilePath(index), jason);
#endif
        }
        private string FileName(int index) => $"{fileName}{index}";
        private string FilePath(int index)
        {
#if UNITY_EDITOR
            return Path.Combine(Application.persistentDataPath, $"{FileName(index)}-editor.json");
#else
            return Path.Combine(Application.persistentDataPath, $"{FileName(index)}.json");
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
