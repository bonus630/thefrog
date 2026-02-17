using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class ActiveByEvent : MonoBehaviour
    {
        [SerializeField] IActivator ItemToActive;
        [SerializeField] GameEventName GameEvent;
        [SerializeField][Tooltip("Desmarque para desativar")] bool Activing = true;
        void Start()
        {
            if (GameManager.Instance.IsEventCompleted(GameEvent))
            {
                Toggle();
            }
            GameManager.Instance.eventManager.GameEventCompleted += EventManager_GameEventCompleted;
        }


        private void OnDisable()
        {
            GameManager.Instance.eventManager.GameEventCompleted -= EventManager_GameEventCompleted;
        }
        private void EventManager_GameEventCompleted(GameEvent obj)
        {
            if (obj.Equals(GameEvent))
            {
                Toggle();
            }
        }
        private void Toggle()
        {

            if (ItemToActive == null)
            {
                Debug.LogError($"ActiveByEvent: ItemToActive é null em {gameObject.name}");
                return;
            }
            if (Activing)
                ItemToActive.Activate();
            else
                ItemToActive.Deactive();
        }
    }
}
