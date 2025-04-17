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
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!start)
            {
                start = true;
                musicSource.CrossFade(BackgroundMusic.AdventureStarts);
                mStageBuilder.Build();
            }
        }
    }
}
