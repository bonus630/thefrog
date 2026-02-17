using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class Destroy : MonoBehaviour
    {
        [SerializeField] AudioClip crackAudio;
        private void OnDestroy()
        {
            ServiceLocator.Instance.Get<AudioEffects>().Play(crackAudio);
        }

    }
}
