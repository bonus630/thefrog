using br.com.bonus630.thefrog.Caracters;
using UnityEngine;
namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyTurtle : EnemyBase
    {
        [SerializeField] Collider2D Eyers;
        [SerializeField] protected LayerMask layerMaskProjectiles;
        // private bool eyesColliding;
        private bool falling;
        protected override void Start()
        {
            rg = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();

        }

        protected override void Update()
        {
            base.Update();
            Debug.DrawLine(topPoint.position, downPoint.position, Color.red);
            var eyesColliding = Physics2D.Linecast(topPoint.position, downPoint.position, layerMaskProjectiles);
            if (frontColliding)
            {
                xDirection *= -1;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
            }
            rg.linearVelocityX = Time.deltaTime * speed * xDirection;
            if (eyesColliding)
            {
                Destroy(eyesColliding.collider.gameObject);
                rg.gravityScale = 1;
                rg.linearVelocityX = 0;
                speed = 0;
                falling = true;
            }
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log(collision.gameObject.layer);
            if (collision.gameObject.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.Hit();
                player.KnockUp(new Vector2(200, 200));
                return;
            }

            if (collision.gameObject.CompareTag("Enemy") && falling)
            {
                Skull skull;
                if (collision.gameObject.transform.parent.gameObject.TryGetComponent<Skull>(out skull))
                {
                    GetComponent<CapsuleCollider2D>().enabled = false;

                    skull.DisableShield();
                    Debug.Log("Estou no collision da tartaruga");
                }
                else
                    Destroy(collision.gameObject);
                Hit(1);
                animator.SetTrigger(HitID);

            }
            if ((collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Destroyable")) && falling)
            {
                Hit(1);
                animator.SetTrigger(HitID);
                if (collision.gameObject.CompareTag("Ground"))
                    return;
                Destroy(collision.gameObject);
            }

        }


        public void Destroy()
        {
            Destroy(gameObject);
        }


    }
}
