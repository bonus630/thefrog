using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyPigPatrolFollowPlayer : EnemyPigPatrol
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private bool detectPlayer;
        [SerializeField] float jumpForce = 10f;
        [SerializeField] Transform sense;
        [SerializeField] float normalSpeed = 20f;
        [SerializeField] float followSpeed = 80f;
        [SerializeField] bool canJump = true;

        float followDistance = 6f;
        float turnDistance = 3f;
        bool jump = false;
        float safeFollowTurn = 0f;
        protected readonly int DeadID = Animator.StringToHash("Dead");
        protected readonly int JumpID = Animator.StringToHash("Jump");
        protected override void Start()
        {
            base.Start();
            normalSpeed += Random.Range(0,4) * 10;
            followSpeed += Random.Range(0,4) * 10;
        }
        protected override void Update()
        {
        
            base.Update();
            safeFollowTurn -= Time.deltaTime;
          //  Debug.DrawRay(new Vector3(transform.position.x - (0.1f * xDirection), transform.position.y - 0.1f, 0), Vector3.left * turnDistance * xDirection, Color.green);
            RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - (0.2f * xDirection), transform.position.y - 0.1f), Vector2.left * turnDistance * xDirection, turnDistance, playerLayer);

            RaycastHit2D detectGround = Physics2D.CircleCast(sense.position, 0.1f,Vector2.down,0.5f,layerMask);

           // Debug.DrawRay(new Vector3(transform.position.x + (1f * xDirection), transform.position.y - 0.1f, 0), Vector3.right * followDistance * xDirection, Color.blue);
            RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + (0.2f * xDirection), transform.position.y - 0.1f), Vector2.right * followDistance * xDirection, followDistance, playerLayer);

            if (hitRight.collider != null)
            {
             //   Debug.Log($"Pig Distance: {hitRight.distance}");
                detectPlayer = true;
                speed = followSpeed;
                hooinkTime = 0.5f;
                if (hitRight.distance > 1.8 && hitRight.distance < 2 && Random.value < 0.4f)
                    Jump();
            }
            if(hitLeft.collider!=null)
            {
               if(safeFollowTurn < 0)
                ChangeDirection();
            }
            if (detectGround.collider == null)
            {
                if (!jump && safeFollowTurn < 0)
                {
                    safeFollowTurn = 1f;
                    ChangeDirection();
                }
            }
            else
            {
                if(rg.linearVelocityY < 0.1f)
                  jump = false;
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
            if(Input.GetKeyDown("q"))
            {
                Jump();
            }
           // Debug.Log($"chao {detectGround.GetContacts(new ContactPoint2D[4])}");
        }
     
        public override void Hit(float hit)
        {
            animator.SetTrigger(HitID);
            this.life = this.life - hit;
        }
        public override void Dead()
        {
            animator.SetFloat(DeadID, this.life);
            if (this.life <= 0)
            {
                xDirection = 0;
                bodyCollider.enabled = false;
                Destroy(gameObject, 0.66f);
            }
        }
        public void Jump()
        {
            if (!canJump)
                return;
            jump = true;
            rg.linearVelocityY = jumpForce;
        }
    }
}
