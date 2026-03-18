using UnityEngine;
using UnityEngine.Events;

namespace br.com.bonus630.thefrog.Shared
{
    public class EventRelay : MonoBehaviour
    {
        [SerializeField] UnityEvent @event;

        public void ActiveEvent()
        {
            @event?.Invoke();
        }
        
        
    }
}
