using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.Rendering;

namespace br.com.bonus630.thefrog.Manager
{
    public class AudioEffects : MonoBehaviour , IService
    {
        [SerializeField] AudioSource audioSource;
        private void Start()
        {
            ServiceLocator.Instance.Register<AudioEffects>(this);
        }

        public void Play(AudioClip audioClipe) => audioSource.PlayOneShot(audioClipe);


    }
}
