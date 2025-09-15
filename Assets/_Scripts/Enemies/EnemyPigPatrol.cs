using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyPigPatrol : EnemyToad
    {
        [SerializeField]protected BoxCollider2D bodyCollider;
        [SerializeField] protected GameObject weapon;
        AudioSource m_AudioSource;
        protected float hooinkTime = 0f;
        protected float runTime = 2f;
        protected float maxRunTime = 2f;
        protected readonly int DeadID = Animator.StringToHash("Dead");
        protected override void Start()
        {
            base.Start();
            m_AudioSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            base.Update();
            if (hooinkTime < 0)
            {
                hooinkTime = Random.Range(1.5f, 4f);
                m_AudioSource.Play();
            }
            hooinkTime -= Time.deltaTime;
        }
        public override void Hit(float hit)
        {
            this.life = this.life - hit;
            if (this.life < 0.1f)
                Dead();
        }
        public virtual void Dead()
        {
            animator.SetFloat(DeadID, this.life);
            if(weapon!=null)
            {
                if(weapon.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
                {
                    rb.gravityScale = 10;
                }
            }
            if (this.life <= 0)
            {
                xDirection = 0;
                bodyCollider.enabled = false;
                Destroy(gameObject, 0.66f);
            }
        }
    }
}
