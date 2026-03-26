using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCVirtualGuy : NPCBase, INPC
    {
        //este npc vai utilizar 16 bit para o indice do dialogo, e 16 bit para marcar os dialogos ja lidos
        [SerializeField] List<DialogueData> dialoguesData;
        /// <summary>
        /// Este é o ultimo dialogo lido, então já podemos passar para o próximo
        /// </summary>
        [SerializeField] private int currentDialogue = 0;
        [SerializeField] private int prevDialogue = 0;
        [SerializeField] private int receivedApples, playerApples = 0;
        [SerializeField] private GameObject heart;
        private int prizeApplesAmount = 50;
        private NpcDialogState dialogState;
        private PackedDialogState packed;
        protected override void Awake()
        {
            base.Awake();
            currentDialogue = DialogStateUtils.GetDialogIndex(GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue);
            currentDialogueData = dialoguesData[currentDialogue];
            for (int i = 0; i < dialoguesData.Count; i++)
            {
                dialoguesData[i].IsReaded = DialogStateUtils.IsDialogRead(GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue, i);
            }
        }
        //este metodo é chamado assim que o player entra na area de interaçaão e ao chamar o proximo dialogo
        public override void CheckDialogs()
        {
            receivedApples = GameManager.Instance.EnvironmentStates.NPCVirtualGuyApples;
            playerApples = GameManager.Instance.PlayerStates.Collectables;
         //   Debug.Log("[NPCVirtualGuy][CheckDialogs] currentDialogue start: " + currentDialogue);
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
           currentDialogue =  EvaluateDialogue();
            currentDialogueData = dialoguesData[currentDialogue];
        }
        private int EvaluateDialogue()
        {
            bool hasSkill =  GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch);
            bool treeFound = GameManager.Instance.IsEventCompleted(GameEventName.AppleTreeFounded);

            bool d0 = dialoguesData[0].IsReaded;
            bool d1 = dialoguesData[1].IsReaded;
            bool d2 = dialoguesData[2].IsReaded;
            bool d3 = dialoguesData[3].IsReaded;
            bool d6 = dialoguesData[6].IsReaded;
            bool d7 = dialoguesData[7].IsReaded;
            bool d8 = dialoguesData[8].IsReaded;


            //bool has10Player = playerApples >= 10;
            bool npcHas50 = receivedApples >= 50;

            if (hasSkill)
                return 5;
            if (npcHas50)
                return 4;
            if(treeFound)
            {
                if (d3)
                {
                    if (d6 || d7)
                        return 8;
                    else
                        return PlayerHasApples();
                }
                else
                    return 3;
            }
            else
            {
                if (!d0)
                    return 0;
                if (!d1)
                    return 1;
                if (!d2)
                    return 2;
                if (d6 || d7)
                    return 2;
                else
                    return PlayerHasApples();
            }

           
        }
        //este metodo é chamado no final da conversa
        private void SetDialog(int dialog, [CallerMemberName] string caller = "")
        {
         //   Debug.Log("[NPCVirtualGuy][SetDialog] currentDialogue: " + currentDialogue + " caller: " + caller);
            dialogueCounter = 0;
            dialoguesData[dialog].IsReaded = true;
            switch (dialog)
            {
                case 0:
                    break;
                case 1:
                    break;
                case 2:
                    dialoguesData[6].IsReaded = false;
                    dialoguesData[7].IsReaded = false;
                    //Este metodo reduz em 10 as maças do player e adiciona ao npc, falha silenciosamente se o player nao possuir 10, o contexto do dialogo permite isso
                    ChangePlayerHearts();
                    break;
                case 3:
                    break;
                case 4:
                     GameManager.Instance.EventCompleted(GameEventName.FeatherTouch);
                    StartCoroutine(CoroutineUtil.WaitFrames(FeatherTouchCompleted, 1));
                    break;
                case 5:
                    break;
                case 6:
                    
                    break;
                case 7:
                   
                    ChangePlayerHearts();
                    break;
                case 8:
                    dialoguesData[6].IsReaded = false;
                    dialoguesData[7].IsReaded = false;
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
        }

        private  void FeatherTouchCompleted()
        {
            Debug.Log("Aqui");
        
            IPlayer player = ServiceLocator.Instance.Get<IPlayer>();
            player.AllInputsOn(false,autoSwitch:true,switchTime: 4.2f);
            player.AddAction(SchedulerData.Do(player.AddForce,Vector2.up * -400,ForceMode2D.Impulse,1f,false,0.8f));
            player.AddAction(SchedulerData.Do(player.FallsControl, 2.2f));
            player.AddAction(SchedulerData.Do(player.UpgradePlayer, 1.8f));
        }

        private void ChangePlayerHearts()
        {
            int amount = GameManager.Instance.PlayerStates.Collectables / 10;
            if (amount > 0)
            {
                GetApples(10);
                Instantiate(heart, new Vector3(transform.position.x - Random.Range(1.3f,2.8f), transform.position.y, transform.position.z), Quaternion.identity);
            }

        }
        private  int PlayerHasApples()
        {
            return playerApples >= 10 ? 7 : 6;
        }
        private void GetApples(int apples)
        {
            GameManager.Instance.PlayerStates.Collectables -= apples;
           // Debug.Log("[NPCVirtualGuy][GetApples] player apples in collectables: " + GameManager.Instance.PlayerStates.Collectables);
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
        public override Transform GetTransform() => transform;
        public override void SetFinishDialogue() => SetDialog(currentDialogue);
        public override void Interact(){  }
        public override Dictionary<string, string> GetDialogueVariables()
        {
            int val = prizeApplesAmount - receivedApples;
            if (val < 0)
                val = 0;
            int hour = ServiceLocator.Instance.Get<IHourProvider>().Hour;
            string greeting = "Boa noite";
            if (hour >= 5 && hour <= 12)
                greeting = "Bom dia";
            else if (hour > 12 && hour <= 18)
                greeting = "Boa tarde";
            return new Dictionary<string, string>() { { "{apples}", $"{val}" },{ "{hour}",greeting} };
        }

        public void CheckInitialDialogue(int dialogue)
        {
            currentDialogue = dialogue;
        }

        public void SetEventsCompleted()
        {
            throw new System.NotImplementedException();
        }
        public override DialogueData GetDialogueForPlayer()
        {
          //  Debug.Log("[NPCVirtualGuy][GetDialogueForPlayer] Iniciando! -------------------------------");
            return base.GetDialogueForPlayer();
        }
    }
}
