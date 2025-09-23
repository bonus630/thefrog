using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCPrisioner : NPCBase, INPC
    {

        [SerializeField] DialogueData OpenedCellDoorDialogue;

        public override DialogueData CurrentDialogueData { get
            {
                if (GameManager.Instance.IsActived("DoorCell_0004"))
                    return OpenedCellDoorDialogue;
                else
                    return dialogueData;
            }
             protected set => base.CurrentDialogueData = value; }

        private void Start()
        {
            if (GameManager.Instance.IsEventCompleted(GameEventName.PrisionerTip))
                Destroy(gameObject);
        }
        public void CheckInitialDialogue(int dialogue)
        {
            
        }

        public override Transform GetTransform()
        {
            return transform;
        }

        public override void Interact()
        {
            
        }
        public override void SetFinishDialogue()
        {
            dialogueCounter = 0;
            if(CurrentDialogueData== OpenedCellDoorDialogue)
            {
                GameManager.Instance.EventCompleted(GameEventName.PrisionerTip);
            }
        }
    
    }
}
