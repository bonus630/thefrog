using System.Collections;
using System.Collections.Generic;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    public class NPCWoman : NPCBase, INPC
    {
        [SerializeField] GameObject[] EntraceList;
        [SerializeField] Collider2D Entrace;
        [SerializeField] MusicSource musicSource;
        [SerializeField] int mazeSteps = 5;
        [SerializeField] DialogueData firstDialogue;
        [SerializeField] DialogueData secondDialogue;
        [SerializeField] DialogueData thirdDialogue;
        [SerializeField] AudioClip lamentMusic;
        List<int> mazeDirections = null;
        private readonly string MAZE = "MAZE";
        private bool createInThisInteraction = false;
        public void Start()
        {
            if(GameManager.Instance.IsEventCompleted(GameEventName.LadyLaments))
                Destroy(gameObject);
            this.CurrentDialogueData = firstDialogue;
            musicSource = ServiceLocator.Instance.Get<MusicSource>();

        }
        public override DialogueData CurrentDialogueData { 
            get { 
                CheckDialogue(); 
                return currentDialogueData;
            }  
            protected set => currentDialogueData = value; }
        public void CheckInitialDialogue(int dialogue)
        {

        }
        public override DialogueData GetDialogueForPlayer()
        {
            CheckDialogue();
            return this.CurrentDialogueData;
        }
        private void CheckDialogue()
        {
            //Debug.Log($"createInThisInteraction:{createInThisInteraction} dialogueCounter{dialogueCounter}");
            if (DataScenePreserver.Instance.Contains(MAZE) && !createInThisInteraction)
            {
                mazeDirections = DataScenePreserver.Instance.Get<ListStorage<int>>(MAZE).Values;
                this.CurrentDialogueData = secondDialogue;
            }
            if (GameManager.Instance.EnvironmentStates.Activeds.Contains("trans_0005"))
            {
                this.CurrentDialogueData = thirdDialogue;
            }
        }
        public override Transform GetTransform() => transform;

        public override void Interact()
        {
        }

        public override void SetFinishDialogue()
        {
            
            if(this.CurrentDialogueData==thirdDialogue)
            {
                StartCoroutine(Cutscene());
                return;
            }
            dialogueCounter = 0;
            if (currentDialogueData == secondDialogue)
            {
                musicSource.Play(BackgroundMusic.Ignition, true);
                Entrace.enabled = true;
            }
        }
        public override bool HaveMoreDialogue()
        {
            bool result = CurrentDialogueData.Count > dialogueCounter;
            dialogueCounter++;
            Debug.Log("[npcwoman] dialogueCounter:" + dialogueCounter);
            if (CurrentDialogueData == firstDialogue && CurrentDialogueData.Count==dialogueCounter)
                createInThisInteraction = false;
            if(CurrentDialogueData == firstDialogue && dialogueCounter >= 15 && Entrace.enabled == false)
            {
                musicSource.Play(BackgroundMusic.Ignition, true);
                Entrace.enabled = true;
            }
            return result;
        }
        public override Dictionary<string, string> GetDialogueVariables()
        {
            if (mazeDirections == null)
            {
                FillPath();
                createInThisInteraction = true;
            }
            string path = string.Empty;
            for (int i = 0; i < mazeSteps; i++)
                path += ((MazeDirections)mazeDirections[i]).ToString() + ", ";
            return new Dictionary<string, string>() { { "{entrada}", path } };
        }
        private void FillPath()
        {
           //Debug.Log("[MAZE] PATH FILL");
            mazeDirections = new List<int>();
            for (int i = 0; i < mazeSteps; i++)
            {
                mazeDirections.Add(UnityEngine.Random.Range(0, 4));
            }
            ListStorage<int> li = new() { Values = mazeDirections };
            DataScenePreserver.Instance.Set<ListStorage<int>>(MAZE, li);
        }
        private IEnumerator Cutscene()
        {
            musicSource.CrossFade(BackgroundMusic.lament,false);
            ServiceLocator.Instance.Get<IPlayer>().AllInputsOn(false);
            GameManager.Instance.ScreenEffects.FadeOut(3f);
            yield return new WaitForSeconds(6f);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
            ServiceLocator.Instance.Get<IPlayer>().AllInputsOn(true);
            yield return new WaitForSeconds(3f);
            GameManager.Instance.EventCompleted(GameEventName.LadyLaments);
            GameManager.Instance.ScreenEffects.FadeIn(4f);
            musicSource.InstantPlay(BackgroundMusic.DarkWind,true);


        }
    }
}

