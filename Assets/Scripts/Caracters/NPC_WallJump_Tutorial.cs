using System;
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

        [SerializeField] private int currentDialogue = 0;
        [SerializeField] private int prevDialogue = -1;

        // private bool CanGoToNext = false;
        private GameObject player;
        //private int dialogueCounter = 0;

        private Animator animator;
        private BoxCollider2D box;
        //Conditions
        public bool firstTalk = false;
        public bool killPig = false;
       [SerializeField] private bool playerCheckWall = false;
        public bool wallJump = false;
        public bool isFarAwayFromNPC = false;
        private Coroutine courotine;

        //public bool FirstTalk { get { return firstTalk; } set { firstTalk = value; SetDialogue(); } }
        //public bool KillPig { get { return killPig; } set { killPig = value; SetDialogue(); } }
        public bool PlayerCheckWall { get { return playerCheckWall; } set { playerCheckWall = value; SetDialogue(); } }

        protected override void Awake()
        {
            base.Awake();
            player = GameObject.Find("Player");
            animator = GetComponent<Animator>();
            box = GetComponent<BoxCollider2D>();
            CheckGameEvents();
        }

        private void CheckGameEvents()
        {

            firstTalk = GameManager.Instance.IsEventCompleted(GameEventName.NPCFirstTalk);
            killPig = GameManager.Instance.IsEventCompleted(GameEventName.KillPig);
            SetDialogue();

            if (GameManager.Instance.IsEventCompleted(GameEventName.NPCTutorial))
            {
                firstTalk = true;
                killPig = true;
                playerCheckWall = true;
                GoToFinal();
            }
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
        public override void SetFinishDialogue()
        {
            Debug.Log("Npc currentDialogue: " + currentDialogue);
            dialogueCounter = 0;
            if (IsFirstDialogue)
            {
                if (currentDialogue == 0)
                {
                    prevDialogue = currentDialogue;
                    GameManager.Instance.EventCompleted(GameEventName.NPCFirstTalk);
                }
                if (currentDialogue == 2)
                {
                    prevDialogue = currentDialogue;
                    GameManager.Instance.EventCompleted(GameEventName.NPCFirstTalk);
                }
                if (currentDialogue == 1)
                {
                    StartWallJumpTutorial();
                    
                }
                IsFirstDialogue = false;

            }
            if (firstTalk && killPig && playerCheckWall && (prevDialogue == 0 || prevDialogue == 2) && courotine == null)
                courotine = StartCoroutine(GoToWallJump());
        }
        private void SetDialogue()
         {
            if (!wallJump)
            {

                if (!firstTalk && !killPig && !playerCheckWall)
                    currentDialogue = 0;
                if (firstTalk && !killPig && !playerCheckWall)
                    currentDialogue = 4;
                if (!firstTalk && killPig && !playerCheckWall)
                    currentDialogue = 2;
                if (firstTalk && killPig && !playerCheckWall)
                {
                    if (prevDialogue == 2)
                        currentDialogue = 5;
                    else
                        currentDialogue = 6;
                }
            }

            this.CurrentDialogueData = dialoguesData[currentDialogue];
            GameManager.Instance.EnvironmentStates.NPC_WallJump_Tutorial = currentDialogue;
        }
        protected override void OnGameEventCompleted(GameEvent gameEvent)
        {
            if (gameEvent.Name.Equals(GameEventName.NPCFirstTalk))
            {
                firstTalk = true;
                SetDialogue();
            }
            if (gameEvent.Name.Equals(GameEventName.KillPig))
            {
                killPig = true;
                SetDialogue();
            }
            if(gameEvent.Name.Equals(GameEventName.NPCTutorial))
            {
                firstTalk = true;
                killPig = true;
                playerCheckWall = true;
                StartCoroutine(GoToFinalRoutine());
            }
            if(gameEvent.Name.Equals(GameEventName.DuckPath))
            {
                Dash();
            }
        }
        public void GoToFinal()
        {
            GameManager.Instance.EventCompleted(GameEventName.NPCTutorial);
            GameManager.Instance.UpdatePlayer();
            StartCoroutine(GoToFinalRoutine());
        }
        IEnumerator GoToFinalRoutine()
        {
            // animator.SetBool("StartTutorial", false);
            animator.enabled = false;
            transform.position = point3.transform.position;
          
            currentDialogue = 3;
            SetDialogue();
            yield return null;
            StartCoroutine(EnableAnimator());
        }
        private IEnumerator GoToWallJump()
        {
            Debug.Log("Coroutine walljump started!");
            bool run = true;
            while (run)
            {
                isFarAwayFromNPC = Vector2.Distance(transform.position, player.transform.position) > 15f && Vector2.Distance(point2.transform.position, player.transform.position) > 15f;
                if (firstTalk && killPig && playerCheckWall && isFarAwayFromNPC)
                {
                    run = false;
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
            dialogueCounter = 0;
            wallJump = true;
            SetDialogue();
            // Debug.Log("Current: " + currentDialogue);
            StartCoroutine(EnableAnimator());
        }
        private void StartWallJumpTutorial()
        {
            Debug.Log("Npc currentDialogue: " + currentDialogue);
            box.enabled = false;
            //  GameManager.Instance.EventCompleted(GameEventName.NPCTutorial);
            animator.enabled = true;
            // animator.applyRootMotion = false;
            animator.SetTrigger("StartTutorial");
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

