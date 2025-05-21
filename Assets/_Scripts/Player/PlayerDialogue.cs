using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Player
{
    [Tooltip("Controla os dialogos e interações do jogador")]
    public class PlayerDialogue : PlayerBase
    {
        private DialogueSystem.DialogueSystem dialogueSystem;
        private INPC npc; 
        private IInteract interacting;
        private ITips tips = null;
        protected override void Awake()
        {
            base.Awake();
            dialogueSystem = FindAnyObjectByType<DialogueSystem.DialogueSystem>();
        }
        private void Update()
        {
            if (interacting != null)
                interacting.ReadyToInteract(Mathf.Abs(transform.position.x - interacting.GetTransform().position.x) < 1.1f && player.WallCheck.IsFaceTo(interacting.GetTransform()));
        }
        public void OnAttack(InputAction.CallbackContext context)
        {

            if (context.performed)
            {

                if (npc != null && Mathf.Abs(transform.position.x - interacting.GetTransform().position.x) < 1.1f && player.WallCheck.IsFaceTo(interacting.GetTransform()))
                {

                    if (interacting is INPC inpc)
                    {

                        if (inpc.HaveMoreDialogue())
                        {
                            dialogueSystem.DialogueData = inpc.CurrentDialogueData;
                            dialogueSystem.DialogueVariables = inpc.GetDialogueVariables();
                            dialogueSystem.Next();
                        }
                        else
                        {

                            inpc.SetFinishDialogue();
                        }
                    }


                }
                else if (interacting != null && Mathf.Abs(transform.position.x - interacting.GetTransform().position.x) < 1.1f && player.WallCheck.IsFaceTo(interacting.GetTransform()))
                {
                    interacting.Interact();
                }
                else if (tips != null)
                {

                    ReadDialogue();
                }
                else
                {

                    player.Launch();
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("NPC"))
            {
                if (collision.gameObject.TryGetComponent<INPC>(out npc))
                {
                    dialogueSystem.DialogueData = npc.CurrentDialogueData;
                    collision.gameObject.TryGetComponent<IInteract>(out interacting);
                }
            }
            if (collision.gameObject.CompareTag("Item"))
            {
                collision.gameObject.TryGetComponent<IInteract>(out interacting);
                Debug.Log("Item trigger enter:" + interacting);
            }
            if (collision.gameObject.CompareTag("Tips"))
            {

                tips = collision.gameObject.GetComponent<ITips>();
                dialogueSystem.DialogueData = tips.GetDialogue();
                tips.AutoPlayer(gameObject);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("NPC"))
            {
                Debug.Log("NPC trigger exit");
                npc = null;
                interacting = null;
                dialogueSystem.ResetDialog();//movendo para playerdialogue
            }
            if (collision.gameObject.CompareTag("Item"))
            {
                Debug.Log("Item trigger exit");
                interacting = null;
            }
            if (collision.gameObject.CompareTag("Tips"))
            {
                //  Debug.Log("tips trigger exit");
                tips = null;
                dialogueSystem.ResetDialog();//movendo para playerdialogue
            }
        }
        public void ReadDialogue()
        {
            dialogueSystem.Next();
        }
        public void SetDialogue(DialogueData dialogue)
        {
            dialogueSystem.DialogueData = dialogue;
        }

        public void ResetDialog()
        {
            dialogueSystem.ResetDialog();
        }
    }
}
