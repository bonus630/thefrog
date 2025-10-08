using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class MusicSaveTime : IActivator
    {
        [SerializeField] string key;
        [SerializeField] MusicSource musicSource;
        public override void Activate()
        {
            musicSource.PreserveMusic(key);
        }

        public override void Deactive()
        {

        }
    }
}
