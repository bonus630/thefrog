using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Collider2D))]
    public class MusicActivator : MonoBehaviour
    {
        [SerializeField] MusicSource musicSource;
        [SerializeField] BackgroundMusic music;
        [SerializeField] bool sleepMusicSource = false;
        [SerializeField] AudioSource extraAudioSource;
        [SerializeField] bool looping = false;
        [SerializeField] AudioClip clip;
       
        bool start = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!start)
            {
                start = true;
                if (sleepMusicSource)
                {
                    musicSource.Sleep();
                    extraAudioSource.clip = clip;
                    extraAudioSource.loop = looping;
                    extraAudioSource.Play();
                }
                else
                {
                    musicSource.WakeUp();
                    musicSource.CrossFade(music);
                }
               
            }
        }
    }
}
