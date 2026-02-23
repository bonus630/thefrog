using System.Collections.Generic;
using br.com.bonus630.thefrog.Activators;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCLightiningSpirit : NPCBase, INPC
    {

        [SerializeField] List<DialogueData> dialoguesData;
        [SerializeField] FactoryCutScene factoryCutScene;
        //[SerializeField] GameObject portal;
        private void Start()
        {
            if (dialoguesData is null or { Count: 0})  return;
            if (GameManager.Instance.IsEventCompleted(GameEventName.FireBall))
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


        public override void SetFinishDialogue()
        {
           // Debug.Log(this.CurrentDialogueData.name);
            if(this.CurrentDialogueData.name.Contains("Last"))
            {
                GameManager.Instance.EventCompleted(GameEventName.LightningBolt);
                factoryCutScene.EndScene();
               // portal.SetActive(true);
          
            }
            //GameManager.Instance.EventCompleted(GameEventName.FireBall);
            //Destroy(gameObject);
        }
       

        public void CheckInitialDialogue(int dialogue)
        {
           
        }
    }
}
