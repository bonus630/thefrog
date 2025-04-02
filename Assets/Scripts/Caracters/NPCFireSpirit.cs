using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCFireSpirit : NPCBase, INPC
    {
        [SerializeField] GameObject ExitDoor;
        public override Transform GetTransform()
        {
            return transform;
        }

        public override void Interact()
        {

        }


        public override void SetFinishDialogue()
        {
            GameManager.Instance.EventCompleted(GameEventName.FireBall);
            ExitDoor.SetActive(true);
            Destroy(gameObject);
        }
        public void Appear()
        {
            gameObject.SetActive(true);
        }

        public void CheckInitialDialogue(int dialogue)
        {
            throw new System.NotImplementedException();
        }
    }
}
