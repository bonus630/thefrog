using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class MusicLoadTime : IActivator
    {
        [SerializeField] string key;
        [SerializeField] MusicSource musicSource;
        public override void Activate()
        {
            musicSource.RestoreMusic(key);
        }

        public override void Deactive()
        {
            
        }
    }
}
