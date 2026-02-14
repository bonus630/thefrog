using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Collider2D))]
    public class MusicActivator : MonoBehaviour
    {
        [SerializeField] MusicSource musicSource;
        [SerializeField] BackgroundMusic music;
        [SerializeField] bool sleepMusicSource = false;
        [SerializeField] bool looping = false;
        [SerializeField] bool instantePlay = false;
        [SerializeField] AudioSource extraAudioSource;
        [SerializeField] AudioClip clip;
       
        bool start = false;
        private void Awake()
        {
            musicSource = ServiceLocator.Instance.Get<MusicSource>();
            Debug.Log("[MusicSource] musicSource:" + musicSource);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!start && collision.CompareTag("Player"))
            {
                start = true;
                if(instantePlay)
                {
                    Debug.Log("[MusicSource] musicSource:" + musicSource);
                    if (clip!=null)
                        musicSource.InstantPlay(clip, looping);
                    else
                        musicSource.InstantPlay(music, looping);
                    return;
                }
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
