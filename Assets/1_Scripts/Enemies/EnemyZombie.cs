using System.Collections;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyZombie : EnemyToad, IElement
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private bool detectPlayer;
        [SerializeField] Transform sense;
        [SerializeField] float normalSpeed = 20f;
        [SerializeField] float followSpeed = 80f;
        [SerializeField] float maxRunTime = 6f;
        [SerializeField] GameObject fire;
        [SerializeField] AudioClip[] audioClips;
        Bounds bounds;
        AudioSource au;
        PolygonCollider2D box;
        float safeFollowTurn = 1f;
        float followDistance = 6f;
        float turnDistance = 3f;
        float runTime = 0;
        float relifeTime = 10f;
        bool rise = false;
        bool walking = false;
       // bool fireHit = false;
        //bool burnning = false;
        protected readonly int RiseID = Animator.StringToHash("Rise");
        protected readonly int LampID = Animator.StringToHash("Lamp");

        public bool isActived { get; set; }

        public Elements GetElement => Elements.Fire;

        public Color ElementColor => Color.red;

        protected override void Start()
        {
            animator = GetComponent<Animator>();
            rg = GetComponent<Rigidbody2D>();
            coll = GetComponent<CapsuleCollider2D>();
            box = GetComponent<PolygonCollider2D>();
            au = GetComponent<AudioSource>();
            normalSpeed += Random.Range(0, 4) * 10;
            followSpeed += Random.Range(0, 4) * 10;
           
           
        }



        protected override void Update()
        {
            relifeTime += Time.deltaTime;
            if (!walking)
                return;
            base.Update();
            safeFollowTurn -= Time.deltaTime;
            //  Debug.DrawRay(new Vector3(transform.position.x - (0.1f * xDirection), transform.position.y - 0.1f, 0), Vector3.left * turnDistance * xDirection, Color.green);
            RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - (0.2f * xDirection), transform.position.y - 0.1f), Vector2.left * turnDistance * xDirection, turnDistance, playerLayer);
            Debug.DrawRay(new Vector2(transform.position.x - (0.2f * xDirection), transform.position.y - 0.1f), Vector2.left * turnDistance * xDirection, Color.red, 5f);
            RaycastHit2D detectGround = Physics2D.CircleCast(sense.position, 0.1f, Vector2.down, 0.5f, layerMask);

            // Debug.DrawRay(new Vector3(transform.position.x + (1f * xDirection), transform.position.y - 0.1f, 0), Vector3.right * followDistance * xDirection, Color.blue);
            RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + (0.2f * xDirection), transform.position.y - 0.1f), Vector2.right * followDistance * xDirection, followDistance, playerLayer);

            if (hitRight.collider != null)
            {
                //   Debug.Log($"Pig Distance: {hitRight.distance}");
                detectPlayer = true;
                speed = followSpeed;

            }
            if (hitLeft.collider != null)
            {
                if (safeFollowTurn < 0)
                {
                    Debug.Log("[zombie][hitLeft]change");
                    ChangeDirection();
                }
            }
            if (detectGround.collider == null)
            {
                if (safeFollowTurn < 0)
                {
                    Debug.Log("[zombie][sense]change");
                    safeFollowTurn = 1f;
                    ChangeDirection();
                }

            }
            if (runTime < 0)
            {
                runTime = maxRunTime;
                speed = normalSpeed;
                detectPlayer = false;
            }
            if (detectPlayer)
            {
                runTime -= Time.deltaTime;

            }
            //if (Input.GetKeyDown("q"))
            //{
            //    Debug.Log($"chao {detectGround.collider}");
            //    Jump();
            //    Debug.Log($"chao {detectGround.collider}");
            //}
        }

        public void Rise()
        {
            if (rise || relifeTime < 10f) return;
            rise = true;
            au.clip = audioClips[Random.Range(0, audioClips.Length)];
            au.PlayDelayed(0.5f);
            animator.SetBool(RiseID, rise);
            animator.SetBool(LampID, (Random.Range(0, 1f) > 0.5f));
            this.coll.enabled = true;
            this.box.enabled = false;
            gameObject.layer = 6;
            this.rg.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
        public void UnRise()
        {
            if (!rise) return;
            relifeTime = 0;
            gameObject.layer = 8;
            rise = false;
            walking = false;
            this.life = 4;
            if (animator != null)
            {
                animator.SetBool(RiseID, rise);
                //this.rg.bodyType = RigidbodyType2D.Kinematic;
                this.box.enabled = true;
                this.coll.enabled = false;
                au.Stop();
            }
            if(this.rg!=null)
            {
                this.rg.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
        public void GoWalk() => walking = true;
        public override void Hit(float hit) { }
       
        public  void Hit(float hit,bool fireHit)
        {
            StartCoroutine(HitEffect(fireHit));
            this.life = this.life - hit;
            if (this.life < 0.1f)
            {
                if (fireHit)
                    StartCoroutine(Burn());
                else
                    UnRise();
                return;
            }
        }
        IEnumerator HitEffect(bool fireHit)
        {
            var sr = GetComponent<SpriteRenderer>();
            if (fireHit)
                sr.material.SetColor("_FlashColor",Color.red);
            else
                sr.material.SetColor("_FlashColor", Color.white);
            sr.material.SetInt("_FlashAmount", 1);
            yield return new WaitForSeconds(0.1f);
            sr.material.SetInt("_FlashAmount", 0);
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {

                if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player) && player.FooterTouching(coll))
                {
                    player.KnockUpOnJump(Repulse);
                    Hit(1,false);
                    return;
                }
            }
            if (collision.gameObject.layer == 12)
            {
                if (collision.gameObject.TryGetComponent<IProjectilies>(out IProjectilies p))
                {
                    if (enemyWeakenesses.Contains(p.GetElement))
                    {
                       // fireHit = true;
                        Hit(2,true);
                    }
                }
            }
        }

        private IEnumerator Burn()
        {
            Debug.Log("[zombie][burn]");
            bounds = GetComponent<SpriteRenderer>().bounds;
            // burnning = true;
            this.rg.linearVelocity = Vector2.zero;
            for (int i = 0; i < 3; i++)
            {
                Vector2 pos = bounds.RandomVector2();
                Instantiate(fire, pos, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);

            }
            yield return null;
            Destroy(gameObject);
        }

        public Elements CanActiveBy() => Elements.Fire;

        public Elements CanDeactiveBy() => Elements.Fire;

        public void ActiveBy(Elements element)
        {

        }

        public void DeactiveBy(Elements element)
        {
        }
    }
}
