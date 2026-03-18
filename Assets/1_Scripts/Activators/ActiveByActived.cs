using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class ActiveByActived : MonoBehaviour
    {
        [SerializeField] IActivator ItemToActive;
        [SerializeField] string activedID;
        [SerializeField][Tooltip("No reverso item ativado vai desativar o IActivator")] bool Reverse = false;
        void Start()
        {
            Active(GameManager.Instance.IsActived(activedID));
            GameManager.Instance.ActiveItemChanged += Instance_ActiveItemChanged;
        }
        private void Instance_ActiveItemChanged(string arg1, bool arg2)
        {
           if(arg1.Equals(activedID))
            {
                Active(arg2);
            }
        }
        private void OnDestroy()
        {
            GameManager.Instance.ActiveItemChanged -= Instance_ActiveItemChanged;
        }
       
        private void Active(bool actived)
        {
            if (actived)
            {
                if (Reverse)
                    ItemToActive.Deactive();
                else
                    ItemToActive.Activate();
            }
            else
            {
                if (Reverse)
                    ItemToActive.Activate();
                else
                    ItemToActive.Deactive();
            }
        }
    }
}
