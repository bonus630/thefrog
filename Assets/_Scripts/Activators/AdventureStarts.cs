using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class AdventureStarts : TipsBase
    {
        [SerializeField] MusicSource musicSource;
        [SerializeField] StageBuilder mStageBuilder;
        bool start = false;
        private void Start()
        {
            if(musicSource == null)
            {
                musicSource = ServiceLocator.Instance.Get<MusicSource>();
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!start)
            {
                start = true;
                musicSource.CrossFade(BackgroundMusic.AdventureStarts);
                mStageBuilder.Build();
               
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            Destroy(gameObject);
        }
        public override DialogueData GetDialogue(int index = -1)
        {
            if (GameManager.Instance.EnvironmentStates.run > 1)
                return dialogues[1];
            else
                return base.GetDialogue(index);
        }
    }
}
