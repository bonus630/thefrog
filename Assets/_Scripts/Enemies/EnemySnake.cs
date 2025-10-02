using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemySnake : EnemyToad
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private bool detectPlayer;
        [SerializeField] float jumpForce = 10f;
        [SerializeField] Transform sense;
        [SerializeField] float normalSpeed = 20f;
        [SerializeField] float followSpeed = 80f;
        [SerializeField] bool canJump = true;
        [SerializeField] Transform bite;

        float followDistance = 6f;
        float turnDistance = 3f;
        float runTime = 4f;
        float maxRunTime = 4f;
        float safeFollowTurn = 1f;
        bool attacking = false;
        protected readonly int DeadID = Animator.StringToHash("Dead");
        protected readonly int AttackID = Animator.StringToHash("Attack");
        protected override void Start()
        {
            base.Start();
            normalSpeed += Random.Range(0, 4) * 10;
            followSpeed += Random.Range(0, 4) * 10;
        }
        protected override void Update()
        {

            base.Update();
            if (attacking)
            {
                Attack();
                if (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Attack"))
                    attacking = false;
            }

            safeFollowTurn -= Time.deltaTime;
            //  Debug.DrawRay(new Vector3(transform.position.x - (0.1f * xDirection), transform.position.y - 0.1f, 0), Vector3.left * turnDistance * xDirection, Color.green);
            // RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - (0.2f * xDirection), transform.position.y - 0.1f), Vector2.left * turnDistance * xDirection, turnDistance, playerLayer);

            RaycastHit2D detectGround = Physics2D.CircleCast(sense.position, 0.1f, Vector2.down, 0.5f, layerMask);

            // Debug.DrawRay(new Vector3(transform.position.x + (1f * xDirection), transform.position.y - 0.1f, 0), Vector3.right * followDistance * xDirection, Color.blue);
            RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + (0.2f * xDirection), transform.position.y - 0.1f), Vector2.right * followDistance * xDirection, followDistance, playerLayer);

            if (hitRight.collider != null)
            {
                //   Debug.Log($"Pig Distance: {hitRight.distance}");
                detectPlayer = true;
                speed = followSpeed;

                if (hitRight.distance > 0.01 && hitRight.distance < 0.4f && !attacking)
                {
                    attacking = true;
                    animator.SetTrigger(AttackID);
                    this.rg.AddForce(Vector2.right * xDirection * 20f, ForceMode2D.Impulse);
                }
            }
            //if (hitLeft.collider != null)
            //{
            //    if (safeFollowTurn < 0)
            //        ChangeDirection();
            //}
            if (detectGround.collider == null)
            {
                Debug.Log("Pig change");
                if (safeFollowTurn < 0)
                {
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
            if (Input.GetKeyDown("q"))
            {
                Attack();
            }
            // Debug.Log($"chao {detectGround.GetContacts(new ContactPoint2D[4])}");
        }

        public override void Hit(float hit)
        {
            animator.SetTrigger(HitID);
            this.life = this.life - hit;
            if (this.life < 0.1f)
                Dead();
        }
        public void Dead()
        {
            gameObject.tag = "Untagged";
            gameObject.layer = 0;
            animator.SetFloat(DeadID, this.life);
            if (this.life <= 0)
            {
                xDirection = 0;

                Destroy(gameObject, 0.66f);
            }
        }
        public void Attack()
        { 
            Collider2D[] colls = Physics2D.OverlapCircleAll(bite.position, 0.04f, playerLayer);
            Debug.Log("Snake bite coll: " + colls.Length);
            foreach (var coll in colls)
            {
                if (coll.gameObject.TryGetComponent<IPlayer>(out IPlayer player))
                {
                    player.CurrentLife = 0;
                    player.Hit();
                }
            }
            //Invoke(nameof(ChangeDirection), 0.4f);
        }
    }
}
