using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class DeactivateComponentByEvent : MonoBehaviour
    {
        [SerializeField] Component component;
        [SerializeField] GameEventName GameEvent;
        [SerializeField] bool destroy = true;
        [SerializeField][Tooltip("Desmarque para desativar")] bool Activing = true;
        void Start()
        {
            if (GameManager.Instance.IsEventCompleted(GameEvent))
            {
                if (destroy)
                    RemoveComponent();
                else
                    DisableComponent();
            }
            GameManager.Instance.eventManager.GameEventCompleted += EventManager_GameEventCompleted;
        }

        private void EventManager_GameEventCompleted(GameEvent obj)
        {
            if (destroy)
                RemoveComponent();
            else
                DisableComponent();
        }
        private void DisableComponent()
        {
            if (component == null)
                return;
            component.gameObject.GetComponent(component.GetType()).GetType().GetProperty("enabled")?.SetValue(component, Activing);
        }
        private void RemoveComponent()
        {
            if(component!=null)
            {
                Destroy(component);
            }
        }
        private void OnDisable()
        {
            GameManager.Instance.eventManager.GameEventCompleted -= EventManager_GameEventCompleted;
        }
    }
}
