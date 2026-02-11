using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class FaderActivator : IActivator
    {

        [SerializeField]float enableTime = 1f;
        [SerializeField]float disableTime = 1f;
        //Enable fade
        public override void Activate()
        {
            ServiceLocator.Instance.Get<ScreenFader>().fadeDuration = enableTime;
            ServiceLocator.Instance.Get<ScreenFader>().FadeOut();
        }
        //Disable fade
        public override void Deactive()
        {
            Debug.Log("[FaderActivator] Deactive");
            ServiceLocator.Instance.Get<ScreenFader>().fadeDuration = disableTime;
            ServiceLocator.Instance.Get<ScreenFader>().FadeIn();
        }
    }
}
