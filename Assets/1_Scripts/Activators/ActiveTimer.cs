using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.Events;

namespace br.com.bonus630.thefrog.Activators
{
    public class ActiveTimer : IActivator
    {
        [SerializeField] float time;
        [SerializeField] bool reverse;
        [SerializeField] UnityAction action;
        public override void Activate()
        {
            if (reverse)
            {
                GameManager.Instance.TimeOverEvent -= Instance_TimeOverEvent;
                GameManager.Instance.StopTimer();
            }
            else
            {
                GameManager.Instance.StartTimer(time, null);
                GameManager.Instance.TimeOverEvent += Instance_TimeOverEvent;
            }
        }

        private void Instance_TimeOverEvent()
        {
            if (action != null)
                action.Invoke();
        }

        public override void Deactive()
        {
            if (reverse)
            {
                GameManager.Instance.StartTimer(time, null);
                GameManager.Instance.TimeOverEvent += Instance_TimeOverEvent;
            }
            else
            {
                GameManager.Instance.TimeOverEvent -= Instance_TimeOverEvent;
                GameManager.Instance.StopTimer();
            }
        }

        private void OnDisable()
        {
            GameManager.Instance.TimeOverEvent -= Instance_TimeOverEvent;
        }
    }
}
