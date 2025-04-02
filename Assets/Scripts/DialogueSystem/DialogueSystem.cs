using System.Collections.Generic;
using System.Data;
using UnityEngine;
namespace br.com.bonus630.thefrog.DialogueSystem
{
    public class DialogueSystem : MonoBehaviour
    {
        int current = 0;
        bool finished = false;

        TextAnimation textAnimation;
        DialogUI dialogueUI;
        DialogStates state;
        public DialogueData DialogueData { get; set; }
        public Dictionary<string, string> DialogueVariables { get; set; }
        private void Awake()
        {
            textAnimation = FindAnyObjectByType<TextAnimation>();
            dialogueUI = FindAnyObjectByType<DialogUI>();

        }
        void Start()
        {
            // textAnimation.TextFinish += OnTextFinish;
            state = DialogStates.DISABLED;

        }

        // Update is called once per frame
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
            // Debug.Log("Next");
            if (current == 0)
                dialogueUI.Enable();
            dialogueUI.SetAvatar(DialogueData.Dialogues[current].Avatar);
            //dialogueUI.SetName(dialogueData.Dialogues[current].Name);
            textAnimation.FullText = ReplaceVariables(DialogueData.Dialogues[current++].text);
            if (DialogueData.Count == current)
            {
                finished = true;
                current = 0;
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
            if (Input.GetKeyDown(KeyCode.E))
            {
                //Debug.Log("Typing");
                textAnimation.Skip();
                state = DialogStates.WAITING;
            }
            // Debug.Log("textAnimation.Finish: "+ textAnimation.Finish);
            if (textAnimation.Finish)
                state = DialogStates.WAITING;
        }
        void Waiting()
        {

            if (Input.GetKeyDown(KeyCode.E))
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
        public void ResetDialog()
        {
            // Debug.Log("Resete");
            dialogueUI.Disable();
            state = DialogStates.DISABLED;
            current = 0;
            finished = false;
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
