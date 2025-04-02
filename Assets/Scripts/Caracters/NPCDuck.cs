using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCDuck : NPCBase, INPC
    {

        private void Awake()
        {
            this.CurrentDialogueData = dialogueData;
        }
        public override void SetFinishDialogue()
        {
            GameManager.Instance.EventCompleted(GameEventName.Gravity);
            Dancing();
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
    }
}
