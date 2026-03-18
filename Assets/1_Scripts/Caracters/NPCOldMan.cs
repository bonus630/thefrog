using System.Collections.Generic;
using br.com.bonus630.thefrog.Activators;
using br.com.bonus630.thefrog.DialogueSystem;
using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCOldMan : NPCBase,INPC
    {
        [SerializeField] List<DialogueData> dialoguesData;
        [SerializeField] PlayerCheckArea checkArea;
        public void CheckInitialDialogue(int dialogue)
        {
            currentDialogueData = dialoguesData[dialogue];
        }

        public override Transform GetTransform()
        {
            return transform;
        }

        public override void Interact()
        {
            throw new System.NotImplementedException();
        }
        public override bool HaveMoreDialogue()
        {
            if (checkArea.Checked)
                currentDialogueData = dialoguesData[1];
            else
                currentDialogueData = dialoguesData[0];
            return base.HaveMoreDialogue();
        }
        public override void SetFinishDialogue()
        {
            dialogueCounter = 0;
           
        }
    }
}
