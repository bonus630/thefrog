using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemySkeleton : EnemyBase
    {
        protected readonly int AwakeID = Animator.StringToHash("Awake");
        protected readonly int AttackID = Animator.StringToHash("Attack");

        private bool awaked = false;
        private bool attacking = false;
        private float prevSpeed;
        private AudioSource audioSource;

        [SerializeField] Transform AttackPoint;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] AudioClip slash;
        [SerializeField] AudioClip bones;


        protected override void Start()
        {
            base.Start();
            prevSpeed = speed;
            audioSource = GetComponent<AudioSource>();
        }

        protected override void Update()
        {
            if (!awaked) return;
            base.Update();
            if (frontColliding)
            {
                ChangeDirection();
            }
            RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + (0.2f * xDirection), transform.position.y - 0.5f), Vector2.right  * xDirection, 1f, playerLayer);

            if (hitRight.collider != null)
            {
                if (hitRight.distance > 0.6f && hitRight.distance < 1f && !attacking)
                {
                    speed = 0;
                    attacking = true;
                    animator.SetTrigger(AttackID);
                }
            }
            rg.linearVelocityX = Time.deltaTime * speed * xDirection;
            animator.SetFloat(WalkID, speed);

        }
        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(new Vector3(transform.position.x + (0.2f * xDirection), transform.position.y - 0.5f,0), new Vector3(transform.position.x + (1f * xDirection), transform.position.y - 0.5f, 0));
            Gizmos.DrawWireSphere(AttackPoint.position, 0.2f);
        }
        protected virtual void ChangeDirection()
        {
            xDirection *= -1;
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
        }

        public void SkeletonAwake()
        {
            if (awaked) return;
            audioSource.PlayOneShot(bones);
            animator.SetBool(AwakeID,true);
            awaked = true;
            coll.enabled = true;
            rg.gravityScale = 1;
            
        }
        public void SkeletonSleep()
        {
            if (!awaked) return;
            audioSource.PlayOneShot(bones);
            animator.SetBool(AwakeID,false);
            awaked = false;
            coll.enabled = false;
            rg.gravityScale = 0;
            rg.linearVelocity = Vector2.zero;
        }

        public void Attack()
        {
            audioSource.PlayOneShot(slash);
            //Vector2 center = new Vector2(0.566f, 0.032f);
            Collider2D[] hits = Physics2D.OverlapCircleAll(AttackPoint.position, 0.2f, playerLayer);
            
            Debug.Log("Contagem de hits: " + hits.Length);
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log("Hit name: " + hits[i].name);
                if (hits[i].CompareTag("Player"))
                {
                    if(hits[i].gameObject.TryGetComponent<IPlayer>(out IPlayer player))
                    {
                        Debug.Log("Hit player: " + player);
                        player.Hit(2);
                        return;
                    }
                }
            }
           
        }
        public void EndAttack()
        {
            attacking = false;
            speed = prevSpeed;
        }
    
    }
}
