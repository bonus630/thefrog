using System.Collections.Generic;
using br.com.bonus630.thefrog.Activators;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCLightiningSpirit : NPCBase, INPC
    {

        [SerializeField] List<DialogueData> dialoguesData;
        [SerializeField] FactoryCutScene factoryCutScene;
        [SerializeField] string[] InputActionName;
        //[SerializeField] GameObject portal;
        private void Start()
        {
            if (dialoguesData is null or { Count: 0})  return;
            if (GameManager.Instance.IsEventCompleted(GameEventName.FireBall))
                if (dialoguesData.Count > 2)
                    this.CurrentDialogueData = GetDialogue();
                else
                    this.CurrentDialogueData = dialoguesData[1];
            else
                this.CurrentDialogueData = dialoguesData[0];
        }

        public override Transform GetTransform()
        {
            return transform;
        }

        public override void Interact()
        {

        }
        public  DialogueData GetDialogue(int index = -1)
        {
            string r = ReplaceInput(dialoguesData[2].Dialogues[0].text, InputActionName);
            var dialogue = new Dialogue { Avatar = dialoguesData[2].Dialogues[0].Avatar, Name = dialoguesData[2].Dialogues[0].Name, text = r };
            var l = new List<Dialogue>();
            l.Add(dialogue);
            return new DialogueData() { Dialogues = l };
        }
        private string ReplaceInput(string text, string[] keys)
        {
            for (int i = 0; i < keys.Length; i++)
            {
                string spriteName = ServiceLocator.Instance.Get<IPlayer>().GetFormattedInputName(keys[i]);
                string spriteText = $"<sprite name=\"{spriteName}\">";
                text = text.Replace($"{{{keys[i]}}}", spriteText);
            }
            return text;
        }

        public override void SetFinishDialogue()
        {
           // Debug.Log(this.CurrentDialogueData.name);
            if(this.CurrentDialogueData.name.Contains("Last"))
            {
                GameManager.Instance.EventCompleted(GameEventName.LightningBolt);
                factoryCutScene.EndScene();
               // portal.SetActive(true);
          
            }
            dialogueCounter = 0;
            if(factoryCutScene.endStep)
                this.CurrentDialogueData = dialoguesData[3];
            else
                this.CurrentDialogueData = dialoguesData[1];
            //GameManager.Instance.EventCompleted(GameEventName.FireBall);
            //Destroy(gameObject);
        }
       

        public void CheckInitialDialogue(int dialogue)
        {
           
        }
    }
}
