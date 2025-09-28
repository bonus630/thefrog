using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.DialogueSystem
{
    public class DialogueSystem : MonoBehaviour
    {
        int current = 0;
        bool finished = false;

        TextAnimation textAnimation;
        public DialogUI dialogueUI { get; protected set; }
        DialogStates state;
        public DialogueData DialogueData { get; set; }
        public Dictionary<string, string> DialogueVariables { get; set; }
        public DialogPosition DialoguePosition { get; set; } = DialogPosition.Bottom;
        [SerializeField] InputAction interactAction;
        private void Awake()
        {
            textAnimation = FindAnyObjectByType<TextAnimation>();
            dialogueUI = FindAnyObjectByType<DialogUI>();

        }
        void Start()
        {
            // textAnimation.TextFinish += OnTextFinish;
            state = DialogStates.DISABLED;
            interactAction.Enable();
        }
        void Update()
        {
            if (state == DialogStates.DISABLED)
                return;
            switch (state)
            {
                case DialogStates.TYPING:
                    Typing();
                    break;
                case DialogStates.WAITING:
                    Waiting();
                    break;
            }
        }
        public void Next()
        {
            if (state != DialogStates.WAITING && state != DialogStates.DISABLED)
                return;
            dialogueUI.SetPosition(DialoguePosition);
            if (current == 0)
                dialogueUI.Enable();

            dialogueUI.SetAvatar(DialogueData.Dialogues[current].Avatar);
            //verificar aqui
           // Debug.Log("Current Dialog System: " + current);
           
           textAnimation.FullText = ReplaceVariables(DialogueData.Dialogues[current].text);
            current++;

            if (DialogueData.Count == current)
            {
                finished = true;
                current = 0;
                dialogueUI.SetHaveMoreIcon(false);
            }
            textAnimation.StartTyping();
            state = DialogStates.TYPING;
        }
        string ReplaceVariables(string text)
        {
            if (DialogueVariables == null || DialogueVariables.Count == 0)
                return text;
            foreach (var variable in DialogueVariables)
            {
                text = text.Replace(variable.Key, variable.Value);
            }

            return text;
        }
        void Typing()
        {
            if (interactAction.WasPressedThisFrame())
            {
                textAnimation.Skip();
                state = DialogStates.WAITING;
            }
            // Debug.Log("textAnimation.Finish: "+ textAnimation.Finish);
            if (textAnimation.Finish)
                state = DialogStates.WAITING;
        }
        void Waiting()
        {

            if (interactAction.WasPressedThisFrame())
            {
                if (finished)
                {
                    //   Debug.Log("waiting if");
                    ResetDialog();
                }
                else
                {
                    // Debug.Log("waiting else");
                    Next();
                }

            }
        }
        int t = 0;
        public void ResetDialog()
        {
             Debug.Log("Resete "+(t++));
            dialogueUI.Disable();
            state = DialogStates.DISABLED;
            current = 0;
            finished = false;
        }
        private void SetPosition(DialogPosition position)
        {
            dialogueUI.SetPosition(position);
        }
        //void OnTextFinish()
        //{
        //    Debug.Log("OnTextFinish");
        //    state = DialogStates.WAITING;
        //}
    }
    public enum DialogStates
    {
        DISABLED,
        WAITING,
        TYPING
    }

}
