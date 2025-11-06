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
                if (Activing)
                    ItemToActive.Activate();
                else
                    ItemToActive.Deactive();
            }
            GameManager.Instance.eventManager.GameEventCompleted += EventManager_GameEventCompleted;
        }
           
        
        private void OnDestroy()
        {
            GameManager.Instance.eventManager.GameEventCompleted -= EventManager_GameEventCompleted;
        }
        private void EventManager_GameEventCompleted(GameEvent obj)
        {
            if (obj.Equals(GameEvent))
            {
                if (Activing)
                    ItemToActive.Activate();
                else
                    ItemToActive.Deactive();
            }
        }
    }
}
