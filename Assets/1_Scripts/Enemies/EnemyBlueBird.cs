using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyBlueBird : EnemyFlyBase
    {
        public void PlayAudioSource()
        {
            audioSource.Play();
        }
        protected override void Update()
        {
            
        }
        public override void Hit(float hit)
        {
            base.Hit(hit);
        }
    }
}
