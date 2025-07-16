using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class MazeDataGetter : MonoBehaviour
    {
        [SerializeField] MazeBuilder mazeBuilder;
        [SerializeField] MusicSource musicSource;
        void Start()
        {
            //Debug.Log(mazeBuilder);
            var li = DataScenePreserver.Instance.Get<ListStorage<int>>("MAZE");
           // var li = GameManager.Instance.GetSceneData<ListStorage<int>>();
            //Debug.Log("Directions li:" + li.Values.Count);
            mazeBuilder.CorrectPath = li.Values;
            //Debug.Log("Directions g:" + mazeBuilder.CorrectPath.Count);
            
            musicSource.Play(BackgroundMusic.Ignition, true);
            musicSource.SavedTime = li.ExtraData;
            musicSource.ResumeMainMusic();
            //musicSource.SavedTime = li.ExtraData;
            //musicSource.ResumeMainMusic();
            // var go = GameObject.Find("DataScenePreserver");
            //
            // mazeBuilder.CorrectPath = go.GetComponent<DataScenePreserver>().Get<List<int>>();
        }

    }
}
