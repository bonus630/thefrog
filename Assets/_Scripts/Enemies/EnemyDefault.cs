
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyDefault : EnemyBase
    {
        [SerializeField] AudioSource audioSource;
        protected override void Update()
        {

        }
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
        }
        public override void Hit(float hit)
        {
            animator.SetTrigger(HitID);
            this.life = this.life - hit;
            if (life < 0.1f)
                Destroy(gameObject, 0.2f);
        }
        public void PlayAudioSource()
        {
            audioSource.Play();
        }
    }
}
