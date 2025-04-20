using System.Collections;
using System.Collections.Generic;
using System.Threading;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCVirtualGuy : NPCBase, INPC
    {
        [SerializeField] List<DialogueData> dialoguesData;
        [SerializeField] private int currentDialogue = 0;
        private int receivedApples = 0;
        private int prizeApplesAmount = 50;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        protected override void Awake()
        {
            base.Awake();
            receivedApples = GameManager.Instance.EnvironmentStates.NPCVirtualGuyApples;
            currentDialogueData = dialogueData;
            Debug.Log("VirtualGuy Dialogue:"+GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue);
        }
        public override Transform GetTransform()
        {
            return transform;
        }
        public override void SetFinishDialogue()
        {
            this.dialoguesData[currentDialogue].IsReaded = true;
            SetDialog(currentDialogue);
            //StartCoroutine(disableCollider());
        }
      
        private void SetDialog(int dialog)
        {
            //Debug.Log("SetFinishDialogue: " + currentDialogue);

            dialogueCounter = 0;

            switch (dialog)
            {
                case 0:
                    if (GameManager.Instance.IsEventCompleted(GameEventName.HeartContainer))
                    {
                        this.dialoguesData[1].IsReaded = true;
                        currentDialogue = 2;
                    }
                    else
                    {
                        currentDialogue = 1;

                    }
                    break;
                case 1:
                    GetApples(GameManager.Instance.PlayerStates.Collectables);
                    if (GameManager.Instance.IsEventCompleted(GameEventName.AppleTreeFounded))
                        currentDialogue = 3;
                    break;
                case 2:
                    ChangePlayerHearts();
                    if (GameManager.Instance.IsEventCompleted(GameEventName.AppleTreeFounded))
                        currentDialogue = 3;
                    break;
                case 3:
                    if (GameManager.Instance.PlayerStates.CollectablesID.Count >= 50 || receivedApples >= 50)
                    {
                        Debug.Log("VirtualGuy apples:" + receivedApples);
                        currentDialogue = 4;
                    }
                    break;
                case 4:
                    if(this.dialoguesData[1].IsReaded)
                        ChangePlayerHearts();
                    GameManager.Instance.EventCompleted(GameEventName.FeatherTouch);
                    GameManager.Instance.UpdatePlayer();
                    currentDialogue = 5;
                    //GetComponent<BoxCollider2D>().enabled = false;
                    break;
                case 5:
                    GetComponent<BoxCollider2D>().enabled = false;
                    break;
            }
            GameManager.Instance.EnvironmentStates.NPCVirtualGuyDialogue = currentDialogue;
            this.CurrentDialogueData = dialoguesData[currentDialogue];
        }
        private void ChangePlayerHearts()
        {
            //Debug.Log("CnangeHearts");
            int amount = GameManager.Instance.PlayerStates.Collectables / 10;
            GetApples(amount * 10);

            GameManager.Instance.UpdateHeart(amount);
            GameManager.Instance.EventCompleted(GameEventName.None);
        }
        private void GetApples(int apples)
        {
            GameManager.Instance.PlayerStates.Collectables -= apples;
            GameManager.Instance.UpdateScore();
            GameManager.Instance.EnvironmentStates.NPCVirtualGuyApples = apples;
            receivedApples += apples;
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
            return new Dictionary<string, string>() { { "{apples}", $"{prizeApplesAmount - receivedApples}" } };
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
