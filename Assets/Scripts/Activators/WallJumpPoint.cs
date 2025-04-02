using br.com.bonus630.thefrog.Caracters;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class WallJumpPoint : MonoBehaviour
    {
        private void OnTriggerExit2D(Collider2D collision)
        {
            FindAnyObjectByType<NPC_WallJump_Tutorial>().PlayerCheckWall = true;
        }

    }
}
