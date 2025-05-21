using br.com.bonus630.thefrog.Shared;
using UnityEngine;


namespace br.com.bonus630.thefrog.Enemies
{
    public class Pig : EnemyBase
    {
        [SerializeField] private AudioClip pigSFX;
        [SerializeField] private AudioClip bossSFX;
        //private int previewLife = 10;
        private AudioSource adu;
        private float furyTimer = 4f;
        //private bool inFury = false;
        //private bool start = false;
        private int flik = 0;
        private new CircleCollider2D collider2D;
      



        // POLYMORPHISM
        protected override void Start()
        {
            this.life = 5;
            //#if UNITY_EDITOR
            //        this.life = 1;
            //#endif
            this.repulse = Vector2.up * 300;
            adu = GetComponent<AudioSource>();
            collider2D = GetComponent<CircleCollider2D>();
            base.Start();
            speed = 0;
            Invoke(nameof(Appear), 0);

        }
        // POLYMORPHISM
        protected override void Update()
        {

            base.Update();
            if (frontColliding)
            {
                ChangeDirection();
            }


        }
        private void ChangeDirection()
        {
            xDirection *= -1;
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
            //anim.SetBool(WalkID, true);
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            rg.linearVelocityX = speed * xDirection;
        }
        protected override void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.CompareTag("Player"))
            {
               
                if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player) && player.FooterTouching(coll))
                {
                    // Debug.Log("Boss Collider " + gameObject.name);
                    Invoke(nameof(Restore), 0.5f);
                    player.KnockUp(repulse);
                    animator.SetTrigger(HitID);
                    this.life--;
                    adu.PlayOneShot(pigSFX);
                    animator.SetBool(RunID, true);
                    animator.SetBool(WalkID, false);
                    speed *= 2;
                    
                    if (life < 1)
                    {
                        Die();
                        return;
                    }
                    Invoke(nameof(ToWalk), furyTimer);
                }
                else
                {
                    ChangeDirection();
                }
                
            }
        }
        private void Die()
        {
            //xDirection = -1;
            IsDied = true;
            // Destroy(gameObject, 2f);
        }
        private void ToWalk()
        {
            animator.SetBool(RunID, false);
            animator.SetBool(WalkID, true);
            speed /= 2;
            speed += 0.4f;
        }
        public override void DestroySelf()
        {
            rg.totalForce = Vector2.zero;
            rg.linearVelocityX = 0f;
            animator.SetBool("Run", false);
            Destroy(gameObject);
        }
        private void Appear()
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
            flik++;
            if (flik < 11)
                Invoke(nameof(Appear), 0.1f);
            else
            {
                speed = 3;
                coll.enabled = true;
                animator.SetBool(WalkID, true);
                adu.PlayOneShot(bossSFX);
            }
        }

    }
}
