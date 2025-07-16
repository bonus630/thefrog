using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class SewerMusicPositionGet : IActivator
    {
        [SerializeField] MusicSource musicSource;

        private void Start()
        {
            if(DataScenePreserver.Instance.Contains("Maze"))
                GetComponent<CircleCollider2D>().enabled = true;    
        }
        public override void Activate()
        {
            if(DataScenePreserver.Instance!=null)
            {
                musicSource.PauseMainMusic();
                DataScenePreserver.Instance.Get<ListStorage<int>>("MAZE").ExtraData = musicSource.SavedTime;
            }
        }

        public override void Deactive()
        {
           
        }

    
    }
}
