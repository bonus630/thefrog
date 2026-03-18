using UnityEngine;
using UnityEngine.Events;

namespace br.com.bonus630.thefrog.Shared
{
    public class GameEventListener : MonoBehaviour
    {
        [SerializeField]private GameEventObject gameEvent;
        [SerializeField] private UnityEvent responseEvent;
        private void OnEnable()=> gameEvent.Register(this);
        private void OnDisable()=> gameEvent.UnRegister(this);

        public void OnRaiseEvent() => responseEvent?.Invoke();
    }
}
