using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.Events;

namespace br.com.bonus630.thefrog.Activators
{
    public class ActiveByUnitEvent : IActivator
    {
        [SerializeField] UnityEvent eventActivation;
        [SerializeField] UnityEvent eventDeactivation;
        public override void Activate()
        {
            if(eventActivation != null)
                eventActivation.Invoke();
        }

        public override void Deactive()
        {
            if (eventDeactivation != null)
                eventDeactivation.Invoke();
        }
    }
}
