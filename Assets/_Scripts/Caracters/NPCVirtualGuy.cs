using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
using UnityEngine.UIElements;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCVirtualGuy : NPCBase, INPC
    {
        //este npc vai utilizar 16 bit para o indice do dialogo, e 16 bit para marcar os dialogos ja lidos
        [SerializeField] List<DialogueData> dialoguesData;
        [SerializeField] private int currentDialogue = 0;
        [SerializeField] private int receivedApples = 0;
        [SerializeField] private GameObject heart;
        private int prizeApplesAmount = 50;
        private NpcDialogState dialogState;
        private PackedDialogState packed;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        protected override void Awake()
        {
            base.Awake();
            Debug.Log("[NPCVirtualGuy][awake] NPCVirtualGuyDialogue: " + GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue);
            receivedApples = GameManager.Instance.EnvironmentStates.NPCVirtualGuyApples;
            //dialogState = DialogStateUtils.Decode(GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue);
            currentDialogue = DialogStateUtils.GetDialogIndex(GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue);
            currentDialogueData = dialoguesData[currentDialogue];
            for (int i = 0; i < dialoguesData.Count; i++)
            {
                dialoguesData[i].IsReaded = DialogStateUtils.IsDialogRead(GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue, i);
            }
            // Debug.Log("VirtualGuy Dialogue:" + GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue);
        }
        public override Transform GetTransform()
        {
            return transform;
        }
        public override void SetFinishDialogue()
        {
            // this.dialoguesData[currentDialogue].IsReaded = true;
            SetDialog(currentDialogue);
            //StartCoroutine(disableCollider());
        }
        //public override void CheckDialogs()
        //{
        //    Debug.Log("[NPCVirtualGuy][CheckDialogs] currentDialogue: " + currentDialogue );
        //    //currentDialogueData = dialoguesData[currentDialogue];
        //    if (currentDialogue == 1 && GameManager.Instance.IsEventCompleted(GameEventName.HeartContainer))
        //        currentDialogue = 2;
        //    if ((currentDialogue == 1 || currentDialogue == 2) && GameManager.Instance.PlayerStates.Collectables == 0)
        //    {
        //        //não posso alterar o indice do dialogo aqui
        //        currentDialogueData = dialoguesData[6];
        //        return;
        //    }
        //    currentDialogueData = dialoguesData[currentDialogue];
        //    //Debug.Log("[NpcVirtualGuy] awake current dialogue: " + currentDialogue);

        //}
        //este metodo é chamado assim que o player entra na area de interaçaão e ao chamar o proximo dialogo
        public override void CheckDialogs()
        {
            Debug.Log("[NPCVirtualGuy][CheckDialogs] currentDialogue start: " + currentDialogue);
            //Dialogo 0, boas vindas, e percebe as maças - precisa ser o primeiro dialogo do npc
            //Dialogo 1, pede as maças,e fala da macieira e transporte - segundo dialogo, entra se o primeiro for lido
            //Dialogo 2, fala sobre os corações, em troca de 10 maças, dialogo longo - terceiro dialogo
            //Dialogo 3, diz quantas maças faltam para a ensinar a habilidade - este vem depois do npc conseguir 10 maças e antes de conseguir 50 e quando o player encontra a arvore
            //Dialogo 4, ensina a habilidade - o npc possuie 50 maças
            //Dialogo 5, não tem mais nada a dizer,  o player ja possuie a habilidade
            //Dialogo 6, pergunta por maças - o npc ainda nao tem 50 maças, mas o player nao tem 10 maças
            //Dialogo 7, oferece um coraçao por 10 maças, dialogo curto - o player tem 10 maças ou mais, e ainda nao tem a habilidade, e o dialogo 2 já foi lido
            //Dialogo 8, diz quantas maças faltam para a ensinar a habilidade - este vem depois do npc conseguir 10 maças e antes de conseguir 50 e quando o player encontra a arvore mas ja viu o dialogo 3
            //somente 2 e 7 fornecem corações
            //if (dialoguesData[0].IsReaded)
            //    currentDialogue = 1;
            //if (dialoguesData[1].IsReaded && GameManager.Instance.PlayerStates.Collectables > 10)
            //    currentDialogue = 2;
            //if (receivedApples >= 10 && receivedApples < 50 && GameManager.Instance.IsEventCompleted(GameEventName.AppleTreeFounded))
            //    currentDialogue = 3;
            //if (receivedApples >= 50 && !GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch))
            //    currentDialogue = 4;
            //if (GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch))
            //    currentDialogue = 5;
            //if (receivedApples < 50 && GameManager.Instance.PlayerStates.Collectables < 10 && dialoguesData[0].IsReaded && dialoguesData[1].IsReaded)
            //    currentDialogue = 6;
            //if (receivedApples >= 10 && receivedApples < 50 && GameManager.Instance.PlayerStates.Collectables >= 10 &&
            //    ((!GameManager.Instance.IsEventCompleted(GameEventName.AppleTreeFounded) &&
            //    dialoguesData[2].IsReaded) || dialoguesData[3].IsReaded || dialoguesData[8].IsReaded))
            //    currentDialogue = 7;
            //if (receivedApples >= 10 && receivedApples < 50 && GameManager.Instance.IsEventCompleted(GameEventName.AppleTreeFounded) && dialoguesData[3].IsReaded)
            //    currentDialogue = 8;


            bool has10Player = GameManager.Instance.PlayerStates.Collectables >= 10;
            bool npcHas10 = receivedApples >= 10;
            bool npcHas50 = receivedApples >= 50;
            bool hasSkill = GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch);
            bool treeFound = GameManager.Instance.IsEventCompleted(GameEventName.AppleTreeFounded);

            bool d0 = dialoguesData[0].IsReaded;
            bool d1 = dialoguesData[1].IsReaded;
            bool d2 = dialoguesData[2].IsReaded;
            bool d3 = dialoguesData[3].IsReaded;

            int result;

            // 5 - Já possui habilidade
            if (hasSkill)
                result = 5;

            // 4 - Ensinar habilidade
            else if (npcHas50)
                result = 4;

            // 10 a 49 maçãs recebidas
            else if (npcHas10)
            {
                // TROCA (2 ou 7)
                if (has10Player)
                    result = d2 ? 7 : 2;

                // FEEDBACK (3 ou 8)
                else if (treeFound)
                    result = d3 ? 8 : 3;

                else
                    result = 6;
            }

            // Antes das 10 maçãs entregues
            else if (d1 && has10Player)
                result = 2;

            else if (d0 && d1 && !has10Player)
                result = 6;

            else if (d0)
                result = 1;

            else
                result = 0;

            currentDialogue = result;
            Debug.Log("[NPCVirtualGuy][CheckDialogs] currentDialogue end: " + currentDialogue);

            currentDialogueData = dialoguesData[currentDialogue];
        }
        //este metodo é chamado no final da conversa
        private void SetDialog(int dialog, [CallerMemberName] string caller = "")
        {
            Debug.Log("[NPCVirtualGuy][SetDialog] currentDialogue: " + currentDialogue + " caller: " + caller);

            dialogueCounter = 0;
            dialoguesData[dialog].IsReaded = true;
            switch (dialog)
            {
                case 0:

                    break;
                case 1:

                    break;
                case 2:
                    ChangePlayerHearts();
                    break;
                case 3:

                    break;
                case 4:

                    GameManager.Instance.EventCompleted(GameEventName.FeatherTouch);
                    GameManager.Instance.GetPlayerScript.UpdatePlayer();

                    break;
                case 5:

                    break;
                case 6:

                    break;
                case 7:
                    ChangePlayerHearts();
                    dialoguesData[7].IsReaded = false;
                    break;
                case 8:

                    break;

            }
            for (int i = 0; i < dialoguesData.Count; i++)
            {
                GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue =
                    dialoguesData[i].IsReaded ?
                    DialogStateUtils.SetDialogRead(
                  GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue, i) :
                  DialogStateUtils.ClearDialogRead(
                     GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue, i);
            }
            GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue = DialogStateUtils.SetDialogIndex(GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue,
                currentDialogue);
            //this.CurrentDialogueData = dialoguesData[currentDialogue];
        }
        //private void SetDialog(int dialog, [CallerMemberName] string caller = "")
        //{
        //    Debug.Log("[NPCVirtualGuy][SetDialog] currentDialogue: " + currentDialogue + " caller: " + caller);

        //    dialogueCounter = 0;

        //    switch (dialog)
        //    {
        //        case 0:
        //            if (GameManager.Instance.IsEventCompleted(GameEventName.HeartContainer))
        //            {
        //                this.dialoguesData[1].IsReaded = true;
        //                currentDialogue = 2;
        //            }
        //            else
        //            {
        //                currentDialogue = 1;
        //            }
        //            break;
        //        case 1:
        //            GetApples(GameManager.Instance.PlayerStates.Collectables);
        //            if (GameManager.Instance.IsEventCompleted(GameEventName.AppleTreeFounded))
        //                currentDialogue = 3;
        //            break;
        //        case 2:
        //            ChangePlayerHearts();
        //            if (GameManager.Instance.IsEventCompleted(GameEventName.AppleTreeFounded))
        //                currentDialogue = 3;
        //            break;
        //        case 3:
        //            if (GameManager.Instance.PlayerStates.CollectablesID.Count >= 50 || receivedApples >= 50)
        //            {
        //                Debug.Log("VirtualGuy apples:" + receivedApples);
        //                currentDialogue = 4;
        //            }
        //            else
        //                currentDialogue = 2;
        //            break;
        //        case 4:
        //            if (this.dialoguesData[1].IsReaded)
        //                ChangePlayerHearts();
        //            GameManager.Instance.EventCompleted(GameEventName.FeatherTouch);
        //            GameManager.Instance.GetPlayerScript.UpdatePlayer();
        //            currentDialogue = 5;
        //            //GetComponent<BoxCollider2D>().enabled = false;
        //            break;
        //        case 5:
        //            GetComponent<BoxCollider2D>().enabled = false;
        //            break;

        //    }
        //    GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue = currentDialogue;
        //    this.CurrentDialogueData = dialoguesData[currentDialogue];
        //}
        private void ChangePlayerHearts()
        {
            //Debug.Log("CnangeHearts");
            int amount = GameManager.Instance.PlayerStates.Collectables / 10;
            if (amount > 0)
            {
                GetApples(10);
                Instantiate(heart, new Vector3(transform.position.x - 2f, transform.position.y, transform.position.z), Quaternion.identity);
                //GameManager.Instance.UpdateMaxHearts(1);
                //GameManager.Instance.EventCompleted(GameEventName.None);
            }

        }
        private void GetApples(int apples)
        {
            GameManager.Instance.PlayerStates.Collectables -= apples;
            Debug.Log("[NPCVirtualGuy][GetApples] player apples in collectables: " + GameManager.Instance.PlayerStates.Collectables);
            GameManager.Instance.UpdateScore();
            receivedApples += apples;
            GameManager.Instance.EnvironmentStates.NPCVirtualGuyApples = receivedApples;
        }
        IEnumerator disableCollider()
        {
            GetComponent<BoxCollider2D>().enabled = false;
            yield return new WaitForSeconds(0.1f);
            GetComponent<BoxCollider2D>().enabled = true;

        }
        public override void Interact()
        {

        }
        public override Dictionary<string, string> GetDialogueVariables()
        {
            int val = prizeApplesAmount - receivedApples;
            if (val < 0)
                val = 0;
            return new Dictionary<string, string>() { { "{apples}", $"{val}" } };
        }

        public void CheckInitialDialogue(int dialogue)
        {
            currentDialogue = dialogue;
        }

        public void SetEventsCompleted()
        {
            throw new System.NotImplementedException();
        }
    }
}
