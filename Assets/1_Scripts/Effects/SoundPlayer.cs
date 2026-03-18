using UnityEngine;

namespace br.com.bonus630.thefrog.Effects
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundPlayer : MonoBehaviour
    {
        AudioSource audioSource;
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void PlaySound()
        {
            audioSource.Play();
        }
     
    }
}
