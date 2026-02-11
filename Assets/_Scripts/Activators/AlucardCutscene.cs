using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.Playables;

namespace br.com.bonus630.thefrog.Activators
{
    public class AlucardCutscene : IActivator
    {
        [SerializeField] PlayableDirector director;
        [SerializeField] MusicSource musicSource;
        [SerializeField] AudioClip clip;
        [SerializeField] float delayTime = 4f;
        float timer = 0;
        bool started = false;
        int hour = -1;
        IHourProvider cycleManager;
        private void Start()
        {
            cycleManager = ServiceLocator.Instance.Get<IHourProvider>();
            hour = cycleManager.Hour;
            cycleManager.OnHourChanged += CycleManager_OnHourChanged;
            musicSource = ServiceLocator.Instance.Get<MusicSource>();
        }

        private void CycleManager_OnHourChanged(int obj)
        {
          //  Debug.Log("alucard: " +obj);
            hour = obj;
        }

        public override void Activate()
        {
            
            this.Actived = true;
        }

        public override void Deactive()
        {
        }
        private void Update()
        {
            
            if(this.Actived && (hour >= 23 || hour <= 2))
            {
                if(!musicSource.IsPlaying(clip))
                    PlayMusic();
                timer += Time.deltaTime;
            }
            if(timer > delayTime && !started)
            {
                started = true;
                
                director.Play();
            }
        }
        private void PlayMusic()
        {
            musicSource.InstantPlay(clip,false);
        }
    }
}
