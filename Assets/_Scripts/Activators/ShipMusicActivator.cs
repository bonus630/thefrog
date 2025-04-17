using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Collider2D))]
    public class MusicActivator : MonoBehaviour
    {
        [SerializeField] MusicSource musicSource;
        [SerializeField] BackgroundMusic music;
       
        bool start = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!start)
            {
                start = true;
                musicSource.CrossFade(music);
               
            }
        }
    }
}
