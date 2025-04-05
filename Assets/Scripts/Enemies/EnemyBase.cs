using System.Collections.Generic;
using br.com.bonus630.thefrog.Caracters;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(Collider2D))]
    public abstract class EnemyBase : MonoBehaviour, IEnemy
    {
        [SerializeField] protected Transform topPoint;
        [SerializeField] protected Transform downPoint;
        [SerializeField] protected LayerMask layerMask;
        [SerializeField] protected Collider2D coll;
        [SerializeField] protected float speed = 200;
        [SerializeField] protected float life = 1;
        [SerializeField] protected Vector2 repulse = Vector2.up * 200;
        [SerializeField] protected float repulseForce = 200;
        protected Rigidbody2D rg;
        protected bool frontColliding;
        protected Animator animator;
        [SerializeField][Range(-1, 1)] protected int xDirection = -1;
        [SerializeField] protected List<Elements> enemyWeakenesses;
        public bool IsEnable { get; set; } = true;

        protected readonly int HitID = Animator.StringToHash("Hit");
        protected readonly int WalkID = Animator.StringToHash("Walk");
        protected readonly int RunID = Animator.StringToHash("Run");

        protected virtual void Awake() { }

        protected virtual void Start()
        {
            rg = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            coll = GetComponent<Collider2D>();
        }

        protected virtual void Update()
        {
            Debug.DrawLine(topPoint.position, downPoint.position, Color.red);
            frontColliding = Physics2D.Linecast(topPoint.position, downPoint.position, layerMask);
            //if(frontColliding)
            //    Debug.Log(xDirection);
        }
        protected virtual void FixedUpdate()
        {

        }
        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player player;
                if (collision.gameObject.TryGetComponent<Player>(out player) && player.FooterTouching(coll))
                {
                    Debug.Log("collision base");
                    player.KnockUp(repulse);
                    Hit(1);
                    return;
                }

            }
            if (collision.gameObject.layer == 12)
            {
                Hit(0.5f);
            }
        }
        public virtual void Hit(float hit)
        {
            //Debug.Log("Collider Hit BASE " + gameObject.name);
            //animator.SetTrigger("Hit");
            animator.SetTrigger(HitID);
            //coll.enabled = false;
            //gameObject.layer = 0;
            //enabled = false;
            IsEnable = false;
            Invoke(nameof(Restore), 1f);
            this.life = this.life - hit;
            if (life < 0.1f)
                Destroy(gameObject, 0.2f);
        }
        public void Restore()
        {
            GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
            enabled = true;
            coll.enabled = true;
            gameObject.layer = 6;
        }
    }
    public enum Elements
    {
        Normal = 0,
        Fire = 1,
        Water = 2,
        Earth = 4,
        Wind = 8
    }
}
