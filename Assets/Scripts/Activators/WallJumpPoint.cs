using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class WallJumpPoint : TipsBase
    {
        private void Start()
        {
        
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
                FindAnyObjectByType<NPC_WallJump_Tutorial>().PlayerCheckWall = true;
        }

        protected override void OnEventCompleted(GameEvent obj)
        {
            if (obj.Name.Equals(GameEventName.NPCTutorial))
                gameObject.SetActive(false);
        }


    }
}
