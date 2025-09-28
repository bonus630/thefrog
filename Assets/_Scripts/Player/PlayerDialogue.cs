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
                canInteract = Mathf.Abs(transform.position.x - interacting.GetTransform().position.x) < 1.1f && player.WallCheck.IsFaceTo(interacting.GetTransform());
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
                    Debug.Log("NPC trigger enter");
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
            dialogueSystem.DialoguePosition = GetDialogPosition();
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
        private DialogPosition GetDialogPosition()
        {
           // Camera cam = GameObject.Find("Hud").GetComponent<Canvas>().worldCamera;
          //  Debug.Log("Camera: " + cam);
            // Usando o método utilitário para verificar se o player está sobre a UI de diálogo
            bool isOverlapping = Utils.UIHelper.IsGameObjectInsideUI(player.gameObject, dialogueSystem.dialogueUI.Rect);
            Debug.Log("Contains : " + isOverlapping);
            if (isOverlapping)
            {
               if(dialogueSystem.DialoguePosition == DialogPosition.Bottom)
                    return DialogPosition.Top;

            }
            return DialogPosition.Bottom;
        }
        //private DialogPosition GetDialogPosition()
        //{
        //    // Obtém a câmera usada pelo Canvas do HUD
        //    Camera cam = GameObject.Find("Hud").GetComponent<Canvas>().worldCamera;
        //    // Obtém a posição do player em coordenadas de tela
        //    Vector3 playerScreenPosition = cam.WorldToScreenPoint(player.transform.position);

        //    // Obtém o RectTransform da UI de diálogo
        //    RectTransform dialogRect = dialogueSystem.dialogueUI.Rect;

        //    // Converte a posição da tela para o espaço do canvas
        //    Canvas canvas = dialogRect.GetComponentInParent<Canvas>();
        //    Vector2 localPoint;
        //    bool inside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
        //        dialogRect, playerScreenPosition, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam, out localPoint
        //    );

        //    // Verifica se o ponto local está dentro do retângulo da caixa de diálogo
        //    bool contains = inside && dialogRect.rect.Contains(localPoint);
        //    Debug.Log("Contains : " + contains);

        //    if (contains)
        //    {
        //        // Se já está no topo, muda para baixo, senão muda para cima
        //        if (dialogueSystem.DialoguePosition.Equals(DialogPosition.Top))
        //            return DialogPosition.Bottom;
        //        else
        //            return DialogPosition.Top;
        //    }
        //    return dialogueSystem.DialoguePosition;
        //}
    }
}
