using System;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class ByHourInterval : MonoBehaviour
    {
        [Range(0, 23)]
        [SerializeField] int activeInitialHour = 0;
        [Range(0, 23)]
        [SerializeField] int activeFinalHour = 0;
        [Range(0, 23)]
        [SerializeField] int deactiveInitialHour = 0;
        [Range(0, 23)]
        [SerializeField] int deactiveFinalHour = 0;

        [SerializeField] IActivator ItemToActive;

        IHourProvider hourProvider;

        void Start()
        {
            hourProvider = ServiceLocator.Instance.Get<IHourProvider>();
            hourProvider.OnHourChanged += HourProvider_OnHourChanged;
        }
        private void OnDisable()
        {
            hourProvider.OnHourChanged -= HourProvider_OnHourChanged;
        }
        private void HourProvider_OnHourChanged(int obj)
        {
            Toggle();
        }

        private void Toggle()
        {
            if(hourProvider.IsInRange(activeInitialHour,activeFinalHour))
                ItemToActive.Activate();
            if (hourProvider.IsInRange(deactiveInitialHour, deactiveFinalHour))
                ItemToActive.Deactive();
        }
    }
}
