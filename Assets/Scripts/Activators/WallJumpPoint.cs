using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class WallJumpPoint : TipsBase
    {
        private void Start()
        {
        
        }
        public override DialogueData GetDialogue(int index = -1)
        {
            if (GameManager.Instance.IsEventCompleted(GameEventName.NPCFirstTalk))
                return dialogues[0];
            else
                return dialogues[1];
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
