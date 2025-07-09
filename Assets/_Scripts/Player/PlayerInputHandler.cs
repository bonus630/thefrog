using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Player
{
    public class PlayerInputHandler : PlayerBase
    {
        public void OnMove(InputAction.CallbackContext context)
        {
            
            player.playerMovement.HandlerMove(context);
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
                player.ChangeGravity(player.gravityDirection * -1);
        }
    }
}
