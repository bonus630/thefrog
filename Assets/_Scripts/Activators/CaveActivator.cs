using UnityEngine;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Manager;

namespace br.com.bonus630.Activators
{
    public class CaveActivator : IActivator
    {
        [SerializeField] MusicSource musicSource;
        [SerializeField] GameObject background;
        [SerializeField] GameObject boss;
        [SerializeField] bool actived = false;
        [SerializeField] BoxCollider2D playerAddForceActivator;
        public override void Activate()
        {
            background.SetActive(actived);
            if (boss != null)
                boss.SetActive(!actived);
            if (actived) 
                musicSource.Play(BackgroundMusic.DarkWind, true);
            else
                musicSource.Play(BackgroundMusic.GoodDayToDie, true);


        }

        public override void Deactive()
        {

        }
    }
}
