
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
       // protected List<GameEventName> gameEvents;
        public DialogueData CurrentDialogueData
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
        protected virtual void Update()
        {

        }
        public bool ReadyToInteract(bool lookFor)
        {
            bool result = lookFor && playerTriggerEnter;
            TalkIcon.SetActive(result);
            return result;
        }


        protected virtual void Awake()
        {
            currentDialogueData = dialogueData;
            GameManager.Instance.eventManager.GameEventCompleted += OnGameEventCompleted;   
            //Debug.Log(currentDialogueData.DialogueName);
        }
        protected virtual void OnDestroy()
        {
            GameManager.Instance.eventManager.GameEventCompleted -= OnGameEventCompleted;
        }
        protected virtual void OnGameEventCompleted(GameEvent gameEvent)
        {

        }
        public void PlayerReadDialogue()
        {
            IsFirstDialogue = false;
        }
        /// <summary>
        /// Utilizar este método para finalizar o dialogo atual e alterar para o proximo dialogo
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

        public virtual Dictionary<string, string> GetDialogueVariables()
        {
            return null;
        }

    }
}
