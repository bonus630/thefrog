using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCAang : NPCBase, INPC
    {
        private Animator anim;
        private Collider2D coll;

        private void Start()
        {
            coll = GetComponent<Collider2D>();
            anim = GetComponent<Animator>();
            if(GameManager.Instance.IsEventCompleted(GameEventName.RollingWind))
            {
                coll.enabled = false;
                TalkIcon.SetActive(false);
            }
        }


        public void CheckInitialDialogue(int dialogue)
        {
           
        }

        public override Transform GetTransform() => transform;

        public override void Interact()
        {
           
        }

        public override bool HaveMoreDialogue()
        {
            bool result = CurrentDialogueData.Count > dialogueCounter;
            dialogueCounter++;
            if(dialogueCounter == 3)
            {
                anim.SetBool("Reverse", false);
                anim.SetTrigger("Awake");
            }
            return result;
        }
        public override void SetFinishDialogue()
        {
            base.SetFinishDialogue();
            anim.SetBool("Reverse", true);
            anim.SetTrigger("Sleep");
            coll.enabled = false;
            TalkIcon.SetActive(false);
            GameManager.Instance.EventCompleted(GameEventName.RollingWind);
            GameManager.Instance.GetPlayerScript.UpdatePlayer();
        }
    }
}
