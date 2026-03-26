using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;    
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
        private bool canInteract = false;
        protected override void Awake()
        {
            base.Awake();
            dialogueSystem = FindAnyObjectByType<DialogueSystem.DialogueSystem>();
        }
        private void Update()
        {
            //temos que melhorar isso aqui
            if (interacting != null)
            {
                canInteract = Mathf.Abs(transform.position.x - interacting.GetTransform().position.x) < 1.2f && player.WallCheck.IsFaceTo(interacting.GetTransform());
                interacting.ReadyToInteract(canInteract);
            }
            else
                canInteract = false;
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {

                if (npc != null && canInteract)
                {

                    if (interacting is INPC inpc)
                    {
                        DialogueData data = inpc.GetDialogueForPlayer();
                        inpc.OnStartDialogue();
                        dialogueSystem.DialogueData = data;
                        dialogueSystem.DialogueVariables = inpc.GetDialogueVariables();
                        if (inpc.HaveMoreDialogue())
                        {
                            dialogueSystem.Next();
                        }
                        else
                        {
                            inpc.SetFinishDialogue();
                        }
                    }
                }
                else if (interacting != null && canInteract)
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
                   // Debug.Log("NPC trigger enter");
                    dialogueSystem.DialogueData = npc.CurrentDialogueData;
                    collision.gameObject.TryGetComponent<IInteract>(out interacting);
                }
            }
            if (collision.gameObject.CompareTag("Item"))
            {
                collision.gameObject.TryGetComponent<IInteract>(out interacting);
                //Debug.Log("Item trigger enter:" + interacting);
            }
            if (collision.gameObject.CompareTag("Tips"))
            {
               // Debug.Log("[PlayerDialogue]tips trigger enter:" + interacting);
               
                SetTip(collision.gameObject.GetComponent<ITips>());
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("NPC"))
            {
                //Debug.Log("NPC trigger exit");
                npc = null;
                interacting = null;
                dialogueSystem.ResetDialog();
            }
            if (collision.gameObject.CompareTag("Item"))
            {
               // Debug.Log("Item trigger exit");
                interacting = null;
            }
            if (collision.gameObject.CompareTag("Tips"))
            {
                //  Debug.Log("tips trigger exit");
                ResetTip();
            }
        }
        public void SetTip(ITips tip)
        {
            this.tips = tip;
            dialogueSystem.DialogueData = tips.GetDialogue();
            tips.AutoPlayer(gameObject);
        }
        public void ResetTip()
        {
            this.tips = null;
            dialogueSystem.ResetDialog();
        }
        public void ReadDialogue()
        {
           // Debug.Log("[PlayerDialogue] readDialogue");
            dialogueSystem.DialoguePosition = GetDialogPosition();
            dialogueSystem.Next();
        }
        public void CancelDialogue()
        {
            npc = null;
            interacting = null;
            tips = null;
            dialogueSystem.ResetDialog(); 
        }
        public void SetDialogue(DialogueData dialogue)
        {
            dialogueSystem.DialogueData = dialogue;
        }

        public void ResetDialog()
        {
            dialogueSystem.ResetDialog();
        }
        private DialogPosition GetDialogPosition()
        {
            bool isOverlapping = Utils.UIHelper.IsGameObjectInsideUI(player.gameObject, dialogueSystem.dialogueUI.Rect);
            //Debug.Log("Contains : " + isOverlapping);
            if (isOverlapping)
            {
               if(dialogueSystem.DialoguePosition == DialogPosition.Bottom)
                    return DialogPosition.Top;
            }
            return DialogPosition.Bottom;
        }

    }
}
