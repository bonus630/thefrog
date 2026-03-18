using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.UI
{
    public class MenuHandler : MonoBehaviour
    {
        [SerializeField] IMenu menu;
        public void GetDirection(InputAction.CallbackContext context)
        {
            menu.GetDirection(context);
        }
        public void ConfirmAction(InputAction.CallbackContext context)
        {
            menu.ConfirmAction(context);
        }
    }
}
