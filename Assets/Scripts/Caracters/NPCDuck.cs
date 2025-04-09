using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCDuck : NPCBase, INPC
    {

        protected override void Awake()
        {
            base.Awake();
            this.CurrentDialogueData = dialogueData;

            CheckGameEvents();
        }

        private void CheckGameEvents()
        {
            if (GameManager.Instance.IsEventCompleted(GameEventName.Gravity))
            {
                Dancing();
            }
        }
        public override void SetFinishDialogue()
        {
            GameManager.Instance.EventCompleted(GameEventName.Gravity);
            GameManager.Instance.UpdatePlayer();

        }

        public override void Interact()
        {

        }

        public override Transform GetTransform()
        {
            return transform;
        }
        public void Dancing()
        {
            GetComponent<Animator>().SetBool("Dancing", true);
            GetComponent<BoxCollider2D>().enabled = false;
        }

        public void CheckInitialDialogue(int dialogue)
        {
            throw new System.NotImplementedException();
        }
        protected override void OnGameEventCompleted(GameEvent gameEvent)
        {
            if (gameEvent.Name.Equals(GameEventName.Gravity))
                Dancing();
        }
    }
}
