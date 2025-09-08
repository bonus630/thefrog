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
        [SerializeField] DialogueData secondDialogue;
        [SerializeField] DialogueData thirdDialogue;
        List<int> mazeDirections = null;
        
        public void Start()
        {
            if(GameManager.Instance.IsEventCompleted(GameEventName.LadyLaments))
                Destroy(gameObject);
           CheckDialogue();

        }
        public override DialogueData CurrentDialogueData { get { CheckDialogue(); return currentDialogueData; }  protected set => currentDialogueData = value; }
        public void CheckInitialDialogue(int dialogue)
        {

        }
        private void CheckDialogue()
        {
            if (DataScenePreserver.Instance.Contains("MAZE"))
            {
                mazeDirections = DataScenePreserver.Instance.Get<ListStorage<int>>("MAZE").Values;
                this.currentDialogueData = secondDialogue;
            }
            if (GameManager.Instance.EnvironmentStates.Activeds.Contains("trans_0005"))
            {
                this.currentDialogueData = thirdDialogue;
            }
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
            if(this.CurrentDialogueData==thirdDialogue)
            {
                GameManager.Instance.ScreenEffects.FadeIn(10f);
                GameManager.Instance.EventCompleted(GameEventName.LadyLaments);
            }
            dialogueCounter = 0;
            musicSource.Play(BackgroundMusic.Ignition, true);
            Entrace.enabled = true;
            // mazeBuilder.ActiveEntrace();

        }
        public override Dictionary<string, string> GetDialogueVariables()
        {
            if (mazeDirections == null)
                FillPath();
            string path = string.Empty;
            for (int i = 0; i < mazeSteps; i++)
                path += ((MazeDirections)mazeDirections[i]).ToString() + ", ";
            return new Dictionary<string, string>() { { "{entrada}", path } };
        }
        private void FillPath()
        {
            Debug.Log("MAZE PATH FILL");
            mazeDirections = new List<int>();
            for (int i = 0; i < mazeSteps; i++)
            {

                mazeDirections.Add(UnityEngine.Random.Range(0, 4));
            }
            ListStorage<int> li = new() { Values = mazeDirections };
            DataScenePreserver.Instance.Set<ListStorage<int>>("MAZE", li);
        }
    }
}

