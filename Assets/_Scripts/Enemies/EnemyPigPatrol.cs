using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyPigPatrol : EnemyToad
    {
        [SerializeField]protected BoxCollider2D bodyCollider;
        AudioSource m_AudioSource;
        float hooinkTime = 0f;
        protected float runTime = 2f;
        protected float maxRunTime = 2f;
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
        }
        public virtual void Dead()
        {
            if (this.life <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
