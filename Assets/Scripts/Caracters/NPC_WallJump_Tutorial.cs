using System.Collections;
using System.Collections.Generic;
using System.Linq;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using TMPro;
using UnityEngine;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPC_WallJump_Tutorial : NPCBase, INPC
    {
        [SerializeField] List<DialogueData> dialoguesData;

        [SerializeField] GameObject point1;
        [SerializeField] GameObject point2;
        [SerializeField] GameObject point3;
        [SerializeField] GameObject point4;

        private int currentDialogue = 0;

        // private bool CanGoToNext = false;
        private GameObject player;
        //private int dialogueCounter = 0;

        private Animator animator;
        private BoxCollider2D box;
        //Conditions
        public bool firstTalk = false;
        public bool killPig = false;
        public bool playerCheckWall = false;
        public bool isFarAwayFromNPC = false;
        private Coroutine courotine;

        public bool FirstTalk { get { return firstTalk; } set { firstTalk = value; SetDialogue(); } }
        public bool KillPig { get { return killPig; } set { killPig = value; SetDialogue(); } }
        public bool PlayerCheckWall { get { return playerCheckWall; } set { playerCheckWall = value; SetDialogue(); } }

        private void Awake()
        {
            player = GameObject.Find("Player");
            animator = GetComponent<Animator>();
            box = GetComponent<BoxCollider2D>();
            SetDialogue();
        }

        //public void PrepareToNext()
        //{
        //    player = GameObject.Find("Player");
        //    CanGoToNext = true;
        //}

        //public void SetEvent(GameEvent gameEvent)
        //{
        //    Debug.Log(gameEvent.Name);
        //    if (gameEvent.Name.Equals(GameEventName.NPCFirstTalk) && IsFirstDialogue)
        //        this.CurrentDialogueData = dialoguesData[0];
        //    else if (gameEvent.Name.Equals(GameEventName.NPCTutorial) && IsFirstDialogue)
        //        this.CurrentDialogueData = dialoguesData[1];
        //    else
        //        this.CurrentDialogueData = dialogueData;
        //}
        //private DialogueData GetByName(string name)
        //{
        //    return dialoguesData.FirstOrDefault(r => r.DialogueName.Equals(name));
        //}

        //public void SetConditionsWallJump(bool firstTalk = false,bool killPig = false,bool playerCheckWall = false, bool isFarAwayFromNPC = false)
        //{
        //    this.firstTalk = firstTalk;
        //    this.killPig = killPig;
        //    this.playerCheckWall = playerCheckWall;
        //    this.isFarAwayFromNPC = isFarAwayFromNPC;
        //    SetDialogue();
        //}
        private void SetDialogue()
        {
            IsFirstDialogue = true;
            this.CurrentDialogueData = dialogueData;
            if (!firstTalk && !killPig && IsFirstDialogue)
                this.CurrentDialogueData = dialoguesData[0];
            if (!firstTalk && killPig && IsFirstDialogue)
                this.CurrentDialogueData = dialoguesData[2];
            if (firstTalk && killPig && IsFirstDialogue)
                this.CurrentDialogueData = dialoguesData[1];
            if (currentDialogue == 2)
                this.CurrentDialogueData = dialoguesData[3];
            if (courotine == null)
                courotine = StartCoroutine(GoToWallJump());
            GameManager.Instance.EnvironmentStates.NPC_WallJump_Tutorial = currentDialogue;
        }

        public void GoToFinal()
        {
            //Chamar este metodo no final da animaçao de wall jump
            StartCoroutine(GoToFinalRoutine());
        }
        IEnumerator GoToFinalRoutine()
        {
            // animator.SetBool("StartTutorial", false);
            animator.enabled = false;
            transform.position = point3.transform.position;
            GameManager.Instance.EventCompleted(GameEventName.NPCTutorial);
            currentDialogue = 2;
            yield return null;
            StartCoroutine(EnableAnimator());
        }
        private IEnumerator GoToWallJump()
        {
            while (currentDialogue == 0)
            {
                isFarAwayFromNPC = Vector2.Distance(transform.position, player.transform.position) > 15f && Vector2.Distance(point2.transform.position, player.transform.position) > 15f;
                if (firstTalk && killPig && playerCheckWall && isFarAwayFromNPC)
                {
                    MoveToWallJump();
                }
                yield return new WaitForEndOfFrame();
            }
        }
        public void MoveToWallJump()
        {
            IsFirstDialogue = true;
            animator.enabled = false;
            transform.position = point2.transform.position;
            currentDialogue = 1;
           // Debug.Log("Current: " + currentDialogue);
            StartCoroutine(EnableAnimator());
        }
        IEnumerator EnableAnimator()
        {
            yield return new WaitForSeconds(0.1f);
            animator.enabled = true;
            box.enabled = true;
        }
        public override void Interact()
        {
            //Debug.Log("Npc: "+count++);
            //IsFirstDialogue = false;
        }

        //public bool HaveMoreDialogue()
        //{
        //    //Debug.Log("Npc count: " + CurrentDialogueData.Count);
        //    //Debug.Log("Npc: " + count + " " + (CurrentDialogueData.Count > count));

        //    bool result = CurrentDialogueData.Count > dialogueCounter;
        //    dialogueCounter++;
        //    return result;
        //}

        public override void SetFinishDialogue()
        {
            //Debug.Log("Npc currentDialogue: "+currentDialogue);
            if (IsFirstDialogue)
            {
                if (currentDialogue == 0)
                {
                    GameManager.Instance.EventCompleted(GameEventName.NPCFirstTalk);
                    FirstTalk = true;
                }
                if (currentDialogue == 1)
                {
                    Debug.Log("Npc currentDialogue: " + currentDialogue);
                    box.enabled = false;
                    //  GameManager.Instance.EventCompleted(GameEventName.NPCTutorial);
                    animator.enabled = true;
                    // animator.applyRootMotion = false;
                    animator.SetTrigger("StartTutorial");
                }
                IsFirstDialogue = false;

            }
            dialogueCounter = 0;
        }
        public void Dash()
        {
            animator.SetTrigger("DashDemonstration");
            //animator.enabled = false;
            //transform.position = point4.transform.position;
            Destroy(gameObject, 2f);

        }
        public override Transform GetTransform()
        {
            return transform;
        }

        public void CheckInitialDialogue(int dialogue)
        {
            currentDialogue = dialogue;
        }
    }
}

