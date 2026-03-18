using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.UI
{
    public interface IMenu 
    {
        void GetDirection(InputAction.CallbackContext context);
        void ConfirmAction(InputAction.CallbackContext context);
    }
}
