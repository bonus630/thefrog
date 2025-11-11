using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class DeactivateComponentByEvent : MonoBehaviour
    {
        [SerializeField] Component component;
        [SerializeField] GameEventName GameEvent;

        void Start()
        {
            if (GameManager.Instance.IsEventCompleted(GameEvent))
            {
                RemoveComponent();
            }
        }

        private void RemoveComponent()
        {
            if(component!=null)
            {
                Destroy(component);
            }
        }
    }
}
