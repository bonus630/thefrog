using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class ShipMusicActivator : MonoBehaviour
    {
        [SerializeField] MusicSource musicSource;
       
        bool start = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!start)
            {
                start = true;
                musicSource.CrossFade(BackgroundMusic.Ship);
               
            }
        }
    }
}
