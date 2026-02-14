using System.Collections.Generic;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    public abstract class NPCBase : MonoBehaviour, IInteract
    {
        [SerializeField] protected GameObject TalkIcon;
        [SerializeField] protected DialogueData dialogueData;
        protected DialogueData currentDialogueData;
        protected bool IsFirstDialogue = true;
        protected int dialogueCounter = 0;
        protected bool playerTriggerEnter = false;


        //este metodo é chamado assim que o player entra na area de interaçaão e ao chamar o proximo dialogo
        /// <summary>
        /// Called when the player enters the NPC's interaction area and when the next dialogue is requested.
        /// Use this method to update the NPC's dialogue (for example, `CurrentDialogueData`) based on game state or events.
        /// </summary>
        public virtual void CheckDialogs()
        {

        }
        public virtual DialogueData CurrentDialogueData
        {
            get
            {
                // IsFirstDialogue = false;
                return currentDialogueData;
            }
            protected set
            {
                if (currentDialogueData != value)
                    IsFirstDialogue = true;
                currentDialogueData = value;
            }
        }
        protected virtual void Update() {  }
        public virtual bool ReadyToInteract(bool lookFor)
        {
            bool result = lookFor && playerTriggerEnter;
            TalkIcon.SetActive(result);
            return result;
        }
        protected virtual void Awake()
        {
            currentDialogueData = dialogueData;
            GameManager.Instance.eventManager.GameEventCompleted += OnGameEventCompleted;   
            GameManager.Instance.GameStatesRestaured += OnGameStatesRestaured;  
            //Debug.Log(currentDialogueData.DialogueName);
        }
        protected virtual void OnDisable()
        {
            GameManager.Instance.eventManager.GameEventCompleted -= OnGameEventCompleted;
            GameManager.Instance.GameStatesRestaured -= OnGameStatesRestaured;
        }
        protected virtual void OnGameEventCompleted(GameEvent gameEvent)
        {

        }
        public virtual DialogueData GetDialogueForPlayer()
        {
            CheckDialogs(); // Aqui o NPC ajusta o diálogo conforme estado
            return CurrentDialogueData;
        }

        public virtual void OnStartDialogue() { }
        public void PlayerReadDialogue()
        {
            IsFirstDialogue = false;
        }
        /// <summary>
        /// Use this method to finish the current dialogue.
        /// </summary>
        public virtual void SetFinishDialogue() { }
        /// <summary>
        /// Verifica se tem mais falas no dialogo atual
        /// </summary>
        /// <returns></returns>
        public virtual bool HaveMoreDialogue()
        {
            bool result = CurrentDialogueData.Count > dialogueCounter;
            dialogueCounter++;
            return result;
        }

        protected void OnTriggerEnter2D(Collider2D collision)
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                playerTriggerEnter = true;


            }
        }
        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                playerTriggerEnter = false;

            }
        }

        private bool CheckCanTalk(bool coll)
        {
            return true;
        }
        public abstract void Interact();
        public abstract Transform GetTransform();
        protected virtual void OnGameStatesRestaured()
        {

        }
        public virtual Dictionary<string, string> GetDialogueVariables()
        {
            return null;
        }

    }
}
