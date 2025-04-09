using br.com.bonus630.thefrog.Caracters;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class WallJumpPoint : TipsBase
    {
        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
                FindAnyObjectByType<NPC_WallJump_Tutorial>().PlayerCheckWall = true;
        }

    }
}
