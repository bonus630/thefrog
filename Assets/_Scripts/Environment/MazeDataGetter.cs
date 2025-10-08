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
        private readonly string MAZE = "MAZE";
        void Start()
        {
            if (!DataScenePreserver.Instance.Contains(MAZE))
                return;
            var li = DataScenePreserver.Instance.Get<ListStorage<int>>(MAZE);
            mazeBuilder.CorrectPath = li.Values;
            musicSource.Play(BackgroundMusic.Ignition, true);
            musicSource.SavedTime = li.ExtraData;
            musicSource.ResumeMainMusic();
        }

        private void Update()
        {
            if(mazeBuilder !=null && mazeBuilder.Completed)
            {
                DataScenePreserver.Instance.Get<ListStorage<int>>(MAZE).Flag = true;
            }
        }

    }
}
