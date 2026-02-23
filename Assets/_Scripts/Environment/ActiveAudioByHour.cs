using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class ActiveAudioByHour : MonoBehaviour
    {
        [SerializeField] AudioClip clip;
        [SerializeField] int hour;
       
        private void Start()
        {
           
            ServiceLocator.Instance.Get<IHourProvider>().OnHourChanged += ActiveAudioByHour_OnHourChanged;
            if (ServiceLocator.Instance.Get<IHourProvider>().Hour == hour)
                ActiveAudioByHour_OnHourChanged(hour);
        }

        private void ActiveAudioByHour_OnHourChanged(int obj)
        {
            if (obj == hour && clip != null)
                if (TryGetComponent<AudioSource>(out AudioSource audioSource))
                    audioSource.PlayOneShot(clip);
                else
                    ServiceLocator.Instance.Get<AudioEffects>().Play(clip);
        }
        private void OnDisable()
        {
            ServiceLocator.Instance.Get<IHourProvider>().OnHourChanged -= ActiveAudioByHour_OnHourChanged;
        }
    }
}
