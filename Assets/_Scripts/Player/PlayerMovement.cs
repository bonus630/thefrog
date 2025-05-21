using System;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Player
{
    public class PlayerMovement : PlayerBase
    {
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private float LinearMaxY = 15;
        [SerializeField] float dashActiveMaxTime = 0.5f;
        [SerializeField] float dashReloadMaxTime = 0.5f;
        [SerializeField] private AudioClip jumpSFX;
        [SerializeField] private ParticleSystem JumpDownParticles;
        [SerializeField] private ParticleSystem DashParticles;
        float dashActiveTimer = 0;
        float dashReloadTimer = 0;

        public float Speed { get { return speed; } set { speed = value; } }
        public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }

        private int jumps = 2;
        private float doubleJumpForce;
        private Vector2 direction;
        private Vector2 DashSpeed = new Vector2(1, 0);
        private float timeInFastFall = 0;
        private float acceleration = 0;
        private bool isJumping;
        private bool doubleJump;
        private bool readyToJump;
        private bool resetFastFall = false;
        private bool isWallSliding;
        private bool canWallJump;

        private readonly float wallSlideSpeed = -0.36f;
        private readonly float wallJumpXForce = 120f;
        private float wallJumpYForce = 220f;
        private readonly float maxTimeInFall = 0.6f;

        protected readonly int WalkID = Animator.StringToHash("Walk");
        //protected readonly int RunID = Animator.StringToHash("Run");
        protected readonly int JumpID = Animator.StringToHash("Jump");
        protected readonly int WallJumpID = Animator.StringToHash("WallJump");
        protected readonly int DoubleJumpID = Animator.StringToHash("DoubleJump");

        protected override void Awake()
        {
            Speed = GameManager.Instance.PlayerStates.Speed;
            jumpForce = GameManager.Instance.PlayerStates.JumpForce;
            base.Awake();
        }
        private void Update()
        {
            isWallSliding = IsWallSliding();
            if (player.WallCheck.CheckGround())
            {
                if (!player.InGround)
                    resetJump();
            }
            else
            {
                player.InGround = false;
                anim.SetBool(JumpID, true);
            }
        }
        void FixedUpdate()
        {
            // Debug.Log("Player RB speedY: " + rb.linearVelocityY);
            //       Debug.DrawLine(transform.position, Vector3.up * gravityDirection * 10);
            if (Mathf.Abs(rb.linearVelocityY * player.gravityDirection) > Mathf.Abs(LinearMaxY))
            {
                timeInFastFall += Time.deltaTime;
                if (resetFastFall)
                {
                    Debug.Log("Foi aqui o danado 1");
                    timeInFastFall = 0;
                    // Physics2D.Raycast(transform.position, Vector2.up * gravityDirection);
                    rb.linearVelocityY = LinearMaxY * player.gravityDirection;
                    resetFastFall = false;
                    FallsControlEffect();
                }
                if (timeInFastFall > maxTimeInFall)
                    player.playerHealth.Die();
            }
            if (player.InputsOn)
                Move();
            Jump();
            if (GameManager.Instance.PlayerStates.HasDoubleJump)
                DoubleJump();
            player.KnockedUp();
            if (GameManager.Instance.PlayerStates.HasWallJump)
                WallSliding();

        }

        private bool IsWallSliding()
        {
            bool falling = false;
            if (player.gravityDirection == 1 && rb.linearVelocityY > 0)
                falling = true;
            if (player.gravityDirection == -1 && rb.linearVelocityY < 0)
                falling = true;
            if (!falling)
                return false;
            return !player.InGround && Mathf.Abs(direction.x) > 0 && player.WallCheck.RightWallCheck();

        }
        public void HandlerJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (player.InGround && !inDash)
                {
                    isJumping = true;
                    jumps--;
                }
                if (player.knockUp)
                {
                    player.knockUpForce *= 10;
                }
                if (isWallSliding)
                {
                    canWallJump = true;

                    Debug.Log("resetFastFall isWallSliding");
                }
                if (doubleJump)
                {
                    readyToJump = true;
                }
                if (GameManager.Instance.PlayerStates.FallsControl && timeInFastFall > 0)
                {
                    resetFastFall = true;
                    Debug.Log("resetFastFall fallcontrol");
                }
            }
            //if (context.performed)
            //    Debug.Log("Jump context perfomed"); 
            if (context.canceled)
            {
                if (rb.linearVelocityY > 0)
                {
                    Debug.Log("Jumps: " + jumps);
                    rb.linearVelocityY *= 0.2f * player.gravityDirection;
                    doubleJump = true;
                    jumps--;
                }
            }
        }
        public void HandlerMove(InputAction.CallbackContext context)
        {
            direction = context.ReadValue<Vector2>();
        }
        bool inDash = false;
        public void HandlerDash(InputAction.CallbackContext context)
        {
            if (GameManager.Instance.PlayerStates.HasDash)
            {
                if (context.started)
                {
                    Debug.Log("InDash true");
                    inDash = true;
                }
                if (context.canceled)
                {
                    Debug.Log("InDash false");
                    inDash = false;
                }
            }
        }
       
        private void DoubleJump()
        {
            if (readyToJump && jumps > 0)
            {

                rb.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse);
                doubleJump = false;
                readyToJump = false;
                anim.SetTrigger(DoubleJumpID);

            }
        }
        private void Jump()
        {
            if (isJumping)
            {
                rb.linearVelocityY = jumpForce;
                audioSource.PlayOneShot(jumpSFX);
                isJumping = false;

            }

        }
        private void resetJump()
        {

            JumpDownEffect();
            player.InGround = true;
            airDash = false;
            doubleJump = false;
            anim.SetBool(JumpID, false);
            jumps = 2;
            timeInFastFall = 0;
        }
        private float accelerationFactor = 0.4f;
        private void Move()
        {
            bool canMove = true;
            if (direction.x == 0)
            {
                if (acceleration > 0)
                    acceleration -= accelerationFactor;
                if (acceleration < 0)
                    acceleration += accelerationFactor;
                if (MathF.Abs(acceleration) < accelerationFactor)
                {
                    canMove = false;
                    //acceleration = 0f;
                    //anim.SetBool(WalkID, false);
                }
            }
            else
            {
                //  Debug.Log("Move: "+direction);
                //Debug.Log("R: " + wallCheck.RightWallCheck() + " " + direction);
                // Debug.Log("L: " + wallCheck.LeftWallCheck() + " " + direction);
                // Debug.Log("Direction:" + direction);
                player.LookFor = direction.x < 0 ? -1 : 1;

                if (direction.x > 0)
                {
                    if (transform.localScale.x < 0)
                    {
                        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

                    }
                    if (player.WallCheck.RightWallCheck())
                    {
                        canMove = false;
                    }
                    else
                    {
                        acceleration += 0.4f;
                        if (acceleration > speed)
                            acceleration = speed;
                    }
                }
                if (direction.x < 0)
                {
                    if (transform.localScale.x > 0)
                    {
                        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    }
                    if (player.WallCheck.RightWallCheck())
                        canMove = false;
                    else
                    {
                        acceleration -= 0.4f;
                        if (acceleration < -speed)
                            acceleration = -speed;
                    }

                }


            }
            Dash(canMove);
            if (canMove)
            {
                // anim.SetBool(WalkID, true);
                //Vector3 moviment = new Vector3(direction, 0, 0);
                //transform.position += moviment * Time.deltaTime * speed;
                //if(Mathf.Abs(rb.linearVelocityX) < speed / 2)
                //   rb.AddForceX(speed * 10 * direction.x,ForceMode2D.Force);
                //else 
                // rb.linearVelocityX = speed * direction.x;
                if (player.InGround)
                    rb.linearVelocityX = acceleration * DashSpeed.x;
                else
                    rb.linearVelocityX = speed * direction.x * DashSpeed.x;

            }
            else
            {
                // if (inGround)
                //  {
                // anim.SetBool(WalkID, false);
                //   }
                rb.linearVelocityX = 0;
                acceleration = 0f;
            }

            anim.SetFloat(WalkID, Mathf.Abs(rb.linearVelocityX));
        }
        bool airDash = false;
        bool firstTimeInDashLoop = false;
        private void Dash(bool canMove)
        {
            if (!canMove || dashActiveTimer >= dashActiveMaxTime || (dashReloadTimer > 0 && !firstTimeInDashLoop) || player.WallCheck.RightWallCheck())
                inDash = false;
            if (inDash)
            {
                if (!airDash)
                {
                    if (player.InGround || isWallSliding)
                        airDash = false;
                    else
                        airDash = true;
                    // ParticleSystem.MainModule main = DashParticles.main;

                    if (player.LookFor < 0)
                    {
                        DashParticles.GetComponent<ParticleSystemRenderer>().flip = Vector3.right;
                        DashParticles.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                        // main.startSpeed = -6;
                    }
                    else
                    {
                        DashParticles.GetComponent<ParticleSystemRenderer>().flip = Vector3.zero;
                        DashParticles.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                        //main.startSpeed = 6;
                    }
                    DashSpeed = new Vector2(8, 0);
                    DashParticles.Play();
                    Debug.Log("Dash here time: " + dashReloadMaxTime);
                    // rb.AddForceX(direction.x * DashSpeed.x,ForceMode2D.Impulse);
                    rb.gravityScale = 0;
                    firstTimeInDashLoop = true;
                }
                dashReloadTimer = dashReloadMaxTime;
                dashActiveTimer += Time.deltaTime;
            }
            if (!inDash)
            {
                DashSpeed = new Vector2(1, 0);
                if (player.gravityDirection > 0)
                    rb.gravityScale = -player.gravityScale;
                else
                    rb.gravityScale = player.gravityScale;
                dashReloadTimer -= Time.deltaTime;
                dashActiveTimer -= Time.deltaTime;
                if (dashReloadTimer < 0)
                    dashReloadTimer = 0;
                if (dashActiveTimer < 0)
                    dashActiveTimer = 0;
                firstTimeInDashLoop = false;
            }
        }
        public void FreezePlayerMove()
        {
            direction.x = 0;
            rb.linearVelocity = Vector2.zero;
            player.InputsOn = false;
            anim.SetFloat(WalkID, 0);
            //anim.SetBool(WalkID, false);
        }
        public void UnFreezePlayerMove()
        {
            player.InputsOn = true;
        }
        private void WallSliding()
        {
            anim.SetBool(WallJumpID, isWallSliding);
            // Debug.Log("IsWallSliding: " + isWallSliding);
            if (isWallSliding)
            {
                rb.linearVelocityY = wallSlideSpeed;
                timeInFastFall = 0;
                if (canWallJump)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(new Vector2(wallJumpXForce * direction.x * -1, wallJumpYForce), ForceMode2D.Impulse);
                    StartCoroutine(player.RemoveInputs());
                }
            }
            else
                canWallJump = false;

        }
       
        public void JumpDownEffect()
        {
            JumpDownParticles.Play();
        }
        public void FallsControlEffect()
        {
            Debug.Log("FallsControleEffect");
        }

        public void GravityChanged()
        {
            jumpForce *= -1;
            wallJumpYForce *= -1;
        }
    }
}
