using br.com.bonus630.thefrog.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Player
{
    public class PlayerInputHandler : PlayerBase
    {
        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 directions = context.ReadValue<Vector2>();
            player.playerMovement.HandlerMove(directions);
           // player.playerSpiritController.SelectProjectile(directions.y);
        }
        public void OnDash(InputAction.CallbackContext context)
        {
           player.playerMovement.HandlerDash(context);  
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
                player.IsJumpPressed = true;
            else if (context.canceled)
                player.IsJumpPressed = false;
            player.playerMovement.HandlerJump(context);
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            player.playerDialogue.OnAttack(context);
        }
        public void OnSpirit(InputAction.CallbackContext context)
        {
            player.LaunchSpirit();
        }
        public void OnHability(InputAction.CallbackContext context)
        {
            if (context.started)
                player.playerMovement.HandlerHability();
               
        }
        public void OnSelect(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (GameManager.Instance.GamePaused)
                    GameManager.Instance.OnCallSave(true);
                else
                    player.playerSpiritController.SelectProjectile(1);
            }
        }
        public void OnVision(InputAction.CallbackContext context)
        {
            Debug.Log("[Player input] context:" + context);
            if(context.performed)
                player.ActiveVision();
        }
    }
}
