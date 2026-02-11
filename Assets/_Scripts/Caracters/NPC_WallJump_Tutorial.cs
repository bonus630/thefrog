using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.Playables;
namespace br.com.bonus630.thefrog.Caracters
{
    public class NPC_WallJump_Tutorial : NPCBase, INPC
    {
        [SerializeField] List<DialogueData> dialoguesData;

        [SerializeField] GameObject point1;
        [SerializeField] GameObject point2;
        [SerializeField] GameObject point3;
        [SerializeField] GameObject point4;

        [SerializeField] GameObject dummy;
        [SerializeField] PlayableDirector goToWallCutscene;
        [SerializeField] PlayableDirector wallJumpCutscene;

        [SerializeField] private int currentDialogue = 0;
        [SerializeField] private int prevDialogue = 0;

        private GameObject player;

        private Animator animator;
        private BoxCollider2D box;
        //Conditions
        public bool firstTalk = false;
        public bool killPig = false;
        [SerializeField] private bool playerCheckWall = false;
        public bool wallJump = false;
        public bool finalRoute = false;
        public bool isFarAwayFromNPC = false;
        private Coroutine courotine;
  
        public bool PlayerCheckWall { get { return playerCheckWall; } set { playerCheckWall = value; SetDialogue(); } }

        protected override void Awake()
        {
            base.Awake();
            player = ServiceLocator.Instance.Get("Player");
            animator = GetComponent<Animator>();
            box = GetComponent<BoxCollider2D>();
            this.CurrentDialogueData = dialoguesData[0];
            CheckGameEvents();
            //if (GameManager.Instance.IsActived(firstTalkKey))
            //    firstTalk = true;
        }
        protected override void Update()
        {
            if (wallJumpCutscene.state == PlayState.Playing)
                GetComponent<SpriteRenderer>().enabled = false;
        }
        protected override void OnGameStatesRestaured()
        {
           // Debug.Log("[npc_walljump_tutorial] OnGameStatesRestaured:");
            CheckGameEvents();
        }
        public override DialogueData GetDialogueForPlayer()
        {
            this.CurrentDialogueData = dialoguesData[currentDialogue];
            //Debug.Log("[npc_walljump_tutorial] currentdialog:" + this.currentDialogueData.name);
            return this.CurrentDialogueData;
        }
        private void CheckGameEvents()
        {
            //Debug.Log("[walljump] gravity:" + GameManager.Instance.IsEventCompleted(GameEventName.Gravity));
            if (GameManager.Instance.IsEventCompleted(GameEventName.Gravity))
            {
                Destroy(gameObject);
                return;
            }
            if (GameManager.Instance.IsEventCompleted(GameEventName.DuckPath))
            {
                Dash();
                return;
            }
            firstTalk = GameManager.Instance.IsEventCompleted(GameEventName.NPCFirstTalk);
            killPig = GameManager.Instance.IsEventCompleted(GameEventName.KillPig);
            playerCheckWall = GameManager.Instance.IsEventCompleted(GameEventName.PlayerCheckWall);
            Debug.Log("[walljump] firstTalk: " + firstTalk + " killPig: " + killPig + " PlayerCheckWall: " +  playerCheckWall);
            // GameManager.Instance.eventManager.GameEventCompleted += OnGameEventCompleted;
            if (GameManager.Instance.IsEventCompleted(GameEventName.NPCTutorial))
            {
                firstTalk = true;
                killPig = true;
                playerCheckWall = true;
                finalRoute = true;
                //GoToFinal();
            }
            SetDialogue();
            //Vamos ver se isso nao vai dar problemas
           // SetFinishDialogue();
            if (firstTalk && killPig && playerCheckWall)
            {
                if (finalRoute)
                    StartCoroutine(GoToFinalRoutine());
                else
                    MoveToWallJump();
            }
        }

        public override void SetFinishDialogue()
        {
           // Debug.Log("Npc currentDialogue: " + currentDialogue);
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
            if (firstTalk && killPig && playerCheckWall && (prevDialogue == 0 || prevDialogue == 2) && courotine == null && !finalRoute)
            {
                GoToWallJumpScene();
                //courotine = StartCoroutine(GoToWallJump());
            }
        }
        private void SetDialogue()
        {

           // Debug.Log($"SetDialogue- CurrentDialog:{currentDialogue} firstTalk:{firstTalk} killPig:{killPig} playerCheckWall:{playerCheckWall}");
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
                if (firstTalk && killPig && playerCheckWall && !finalRoute)
                {
                    prevDialogue = 0;
                    currentDialogue = 6;
                }
            }

            this.CurrentDialogueData = dialoguesData[currentDialogue];
            GameManager.Instance.EnvironmentStates.NPC_WallJump_Tutorial = currentDialogue;
        }

        protected override void OnGameEventCompleted(GameEvent gameEvent)
        {
            // Debug.Log("[EventComplet][npc walljump]:" + gameEvent.Name);
            if (gameEvent.Name.Equals(GameEventName.PlayerCheckWall))
            {
                PlayerCheckWall = true;
                SetDialogue();
            }
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
            if (gameEvent.Name.Equals(GameEventName.NPCTutorial))
            {
                firstTalk = true;
                killPig = true;
                playerCheckWall = true;
                finalRoute = true;
                StartCoroutine(GoToFinalRoutine());
            }
            if (gameEvent.Name.Equals(GameEventName.DuckPath))
            {
                Dash();
            }
        }
        public void GoToFinal()
        {
            GameManager.Instance.EventCompleted(GameEventName.NPCTutorial);
            ServiceLocator.Instance.Get<IPlayer>().UpdatePlayer();
            finalRoute = true;
            GetComponent<SpriteRenderer>().enabled = true;
            StartCoroutine(EnableAnimator());
            //GameManager.Instance.GetPlayerScript.UpdatePlayer();
            //  var g = Instantiate(gameObject);
            // g.GetComponent<Animator>().enabled = true;
            //  g.GetComponent<Animator>().SetTrigger("Idle");
            //  g.GetComponent<BoxCollider2D>().enabled = true;
            //   g.GetComponent<NPC_WallJump_Tutorial>().currentDialogueData = dialoguesData[3];
            //   Debug.Log("Nova instancia do npc:" + g);
            //   Destroy(gameObject, 0.2f);
            //StartCoroutine(GoToFinalRoutine());
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
        private void GoToWallJumpScene()
        {
            GetComponent<SpriteRenderer>().enabled = false;
            MoveToWallJump();
            goToWallCutscene.Play();
            GetComponent<SpriteRenderer>().enabled = true;
        }
        private IEnumerator GoToWallJump()
        {
            //if (finalRoute)
            //    yield return null;
            Debug.Log("Coroutine walljump started!");
            bool run = true;
            while (run)
            {
                isFarAwayFromNPC = Vector2.Distance(transform.position, player.transform.position) > 15f && Vector2.Distance(point2.transform.position, player.transform.position) > 15f;
                if (firstTalk && killPig && playerCheckWall && isFarAwayFromNPC)
                {
                    Debug.Log("Coroutine if!");
                    run = false;
                }
                yield return new WaitForEndOfFrame();
            }
            MoveToWallJump();
        }
        public void MoveToWallJump([CallerMemberName] string caller = "")
        {
           
           // Debug.Log("Coroutine MoveTowalljump: " + caller);
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
           // Debug.Log("Npc currentDialogue: " + currentDialogue);
            TalkIcon.SetActive(false);
            ServiceLocator.Instance.Get<IPlayer>().AllInputsOn(true, 1f);
            box.enabled = false;
            ChangeDummy(true);
           
            //  GameManager.Instance.EventCompleted(GameEventName.NPCTutorial);
            //animator.enabled = false;
            // animator.applyRootMotion = false;
            // animator.SetTrigger("StartTutorial");
        }
        IEnumerator EnableAnimator()
        {
            yield return new WaitForSeconds(0.1f);
            while (Vector2.Distance(point2.transform.position, player.transform.position) < 5f)
            {
                yield return null;
            }
            animator.enabled = true;
            box.enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
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
        public override Dictionary<string, string> GetDialogueVariables()
        {
            string msg = string.Empty;
            switch (ServiceLocator.Instance.Get<IHourProvider>().Hour)
            {
                case < 6:
                    msg = ", seja paciente e aguarde até o amanhecer";
                    break;
                case > 17:
                    msg = ", está não é uma boa hora para visita-ló, espere até amanhã";
                    break;
                case > 14 and <= 17:
                    msg = ", RÁPIDO você não tem muito tempo!!!";
                    break;
            }
            Debug.Log("[npc_walljump_tutorial] hour msg: " + msg);
            return new Dictionary<string, string>() { { "{hours}", msg } };
        }
        public void Dash()
        {
            Debug.Log("Npc_tutorial Dash:");
            animator.enabled = true;
            animator.SetTrigger("DashDemonstration");
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

        private void ChangeDummy(bool activeDummy = true)
        {
           // Debug.Log("Dummy:" + activeDummy);
            //dummy.SetActive(activeDummy);
            //box.enabled = !activeDummy;
            //GetComponent<SpriteRenderer>().enabled = !activeDummy;
            if (activeDummy)
            {

               // wallJumpCutscene.time = 1;
              //  wallJumpCutscene.Evaluate();
                wallJumpCutscene.stopped += (d) =>
                {
                  
                    GoToFinal();//npc chegou ao ponto final
                   // // ChangeDummy(false);
                };
                //wallJumpCutscene.played += (d) => {
                //    //move npc para o ponto final
                //};
                StartCoroutine(De());
            }
        }

        private IEnumerator De()
        {
          //  animator.enabled = false;
          //  GetComponent<SpriteRenderer>().color = Color.green;
            StartCoroutine(GoToFinalRoutine());
            yield return new WaitForEndOfFrame();
            wallJumpCutscene.Play();
            yield return new WaitForEndOfFrame();
            
        }

        
    }
}

