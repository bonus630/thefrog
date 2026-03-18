using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class EnableDisableComponentByActiveKey : MonoBehaviour
    {
        [SerializeField] Component component;
        [SerializeField] string checkedID;
        void Start()
        {
            if (GameManager.Instance.IsActived(checkedID))
                SetEnabled(false);
            GameManager.Instance.ActiveItemChanged += Instance_ActiveItemChanged;
        }

        private void Instance_ActiveItemChanged(string id, bool enabled)
        {
            if(id.Equals(checkedID))
                SetEnabled(enabled);
        }

        private void OnDisable()
        {
            GameManager.Instance.ActiveItemChanged -= Instance_ActiveItemChanged;
        }
        private void SetEnabled(bool enabled)
        {
            if(component!=null)
            {
                component.gameObject.GetComponent(component.GetType()).GetType().GetProperty("enabled")?.SetValue(component, enabled);
            }
        }
    }
}
