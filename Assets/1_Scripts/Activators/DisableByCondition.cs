using UnityEngine;
using UnityEngine.Events;

namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Manager.ConditionsManager))]
    public class DisableByCondition : MonoBehaviour
    {
        [SerializeField] UnityEvent conditionEvent;

        private void Start()
        {
            conditionEvent.Invoke();
        }
    }
}