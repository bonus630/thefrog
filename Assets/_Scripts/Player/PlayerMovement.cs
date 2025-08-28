using System;
using System.Collections;
using br.com.bonus630.thefrog.Effects;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
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
        [SerializeField] private ParticleSystem FastFallParticles;
        float dashActiveTimer = 0;
        float dashReloadTimer = 0;
        float coyouteTime = 0.2f;
        float coyouteTimer = 0;
        public float Speed { get { return speed; } set { speed = value; } }
        public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }
        public float TimeInFastFall { get; set; } = 0;
        public bool UseYvelocityLimit { get; set; } = true;

        private int jumps = 2;
        private float doubleJumpForce;
        private Vector2 direction;
        private Vector2 DashSpeed = new Vector2(1, 0);
        private float acceleration = 0;
        private bool isJumping;
        private bool doubleJump;
        private bool readyToJump;
        private bool resetFastFall = false;
        public bool GetWallSliding { get; private set; }
        private bool canWallJump;
        bool inDash = false;
        bool airDash = false;
        bool firstTimeInDashLoop = false;

        //private IEffects bounce;

        private float accelerationFactor = 0.4f;
        private readonly float wallSlideSpeed = -0.36f;
        private readonly float wallJumpXForce = 120f;
        private float wallJumpYForce = 220f;
        private readonly float maxTimeInFall = 0.4f;

        protected readonly int WalkID = Animator.StringToHash("Walk");
        //protected readonly int RunID = Animator.StringToHash("Run");
        protected readonly int JumpID = Animator.StringToHash("Jump");
        protected readonly int WallJumpID = Animator.StringToHash("WallJump");
        protected readonly int DoubleJumpID = Animator.StringToHash("DoubleJump");
        protected readonly int FallingID = Animator.StringToHash("Falling");

        protected override void Awake()
        {
            Speed = GameManager.Instance.PlayerStates.Speed;
            jumpForce = GameManager.Instance.PlayerStates.JumpForce;
            dashReloadTimer = dashReloadMaxTime;
            base.Awake();
        }
        //private void Start()
        //{
        //    bounce = new BounceEffect(anim.gameObject.transform);
        //}
        private void Update()
        {
            GetWallSliding = IsWallSliding();
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
            if (player.InGround)
                coyouteTimer = coyouteTime;
            else if(!GetWallSliding)
            {
                coyouteTimer -= Time.deltaTime;
                anim.SetBool(FallingID, (IsFalling() && !player.playerFallControl.InFallControl));
            }
            DashBarController();
           // Debug.Log($"DashReloadTime: {dashReloadTimer} inDash: {inDash}");

        }
        void FixedUpdate()
        {
            if (Mathf.Abs(player.RigibodyLinearVelocity.y * player.gravityDirection) > Mathf.Abs(LinearMaxY))
            {
                TimeInFastFall += Time.deltaTime;
                if (resetFastFall)
                {
                    FallsControl();
                }
                player.playerHealth.PrepareFallDie = TimeInFastFall > maxTimeInFall;
                if (UseYvelocityLimit)
                {
                    Vector2 velocity = player.RigibodyLinearVelocity;
                    velocity.y = (Mathf.Abs(LinearMaxY) + 4) * player.gravityDirection;
                    player.RigibodyLinearVelocity = velocity;
                }
                var m = FastFallParticles.main;
                m.gravityModifierMultiplier = 0.5f * Mathf.Sign(player.RigibodyLinearVelocityY);
                FastFallParticles.Play();
            }
            else
            {
                FastFallParticles.Stop();
            }
            if (player.MoveInputOn)
                Move();
            Jump();
            if (GameManager.Instance.PlayerStates.HasDoubleJump)
                DoubleJump();
            player.KnockedUp();
            if (GameManager.Instance.PlayerStates.HasWallJump)
                WallSliding();
        }

        public void FallsControl()
        {
            resetFastFall = false;
            player.playerHealth.PrepareFallDie = false;
            Debug.Log("Playermovement fallscontrol");
            player.playerFallControl.FallsControl(true);

        }


        private bool IsWallSliding()
        {
            bool falling = IsFalling();
            bool prepareWall = falling && Mathf.Abs(direction.x) > 0 && player.WallCheck.RightWallCheck();
            if (prepareWall)
            {
                Debug.Log("Player graviti:" + player.gravityDirection);
                float angle = 0;
                if (player.WallCheck.NearGround(out angle, player.gravityDirection))
                {
                    return false;
                }
                return true;
            }
            return false;
        }
        public bool IsFalling()
        {
            bool falling = false;
            if (player.gravityDirection == 1 && player.RigibodyLinearVelocity.y > 0)
                falling = true;
            if (player.gravityDirection == -1 && player.RigibodyLinearVelocity.y < 0)
                falling = true;
            return falling && !player.InGround;
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
                if (GetWallSliding)
                {
                    canWallJump = true;

                    //Debug.Log("resetFastFall isWallSliding");
                }
                if (doubleJump)
                {
                    readyToJump = true;
                }
                //ativa o controle de queda
                if (GameManager.Instance.PlayerStates.FallsControl && TimeInFastFall > 0)
                {
                    resetFastFall = true;
                    //Debug.Log("resetFastFall fallcontrol");
                }
            }
            //if (context.performed)
            //    Debug.Log("Jump context perfomed"); 
            if (context.canceled)
            {
                if (player.RigibodyLinearVelocity.y > 0)
                {
                    //Debug.Log("Jumps: " + jumps);
                    player.RigibodyLinearVelocityY *= 0.2f * player.gravityDirection;
                    doubleJump = true;
                    jumps--;
                }
            }
        }
        public void HandlerMove(Vector2 direction)
        {
            this.direction = direction;

        }
        public void HandlerDash(InputAction.CallbackContext context)
        {
            if (GameManager.Instance.PlayerStates.HasDash)
            {
                if (context.started)
                {
                    //Debug.Log("InDash true");
                    inDash = true;
                }
                if (context.canceled)
                {
                    //Debug.Log("InDash false");
                    inDash = false;
                }
            }
        }

        private void DoubleJump()
        {
            if (readyToJump && jumps > 0)
            {
                player.AddForce(Vector2.up * doubleJumpForce, ForceMode2D.Impulse, 0, false);
                doubleJump = false;
                readyToJump = false;
                anim.SetTrigger(DoubleJumpID);
            }
        }
        private void Jump()
        {
            if (isJumping && coyouteTimer > 0)
            {
                player.RigibodyLinearVelocityY = jumpForce;
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
            TimeInFastFall = 0;
            player.playerFallControl.FallsControl(false);
            anim.SetBool(FallingID, false);
        }
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
                //Debug.Log("Move: "+direction);
                //Debug.Log("R: " + wallCheck.RightWallCheck() + " " + direction);
                //Debug.Log("L: " + wallCheck.LeftWallCheck() + " " + direction);
                //Debug.Log("Direction:" + direction);
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
                    player.RigibodyLinearVelocityX = acceleration * DashSpeed.x;
                else
                    player.RigibodyLinearVelocityX = speed * direction.x * DashSpeed.x;

            }
            else
            {
                // if (inGround)
                //  {
                // anim.SetBool(WalkID, false);
                //   }
                player.RigibodyLinearVelocityX = 0;
                acceleration = 0f;
            }

            anim.SetFloat(WalkID, Mathf.Abs(player.RigibodyLinearVelocityX));
        }
        GameObject dashBar;
        private void Dash(bool canMove)
        {
            //Debug.Log(canMove);
            //Debug.Log(dashActiveTimer >= dashActiveMaxTime);
            //Debug.Log((dashReloadTimer < dashReloadMaxTime && !firstTimeInDashLoop));
            //Debug.Log(player.WallCheck.RightWallCheck());
           // if (!canMove || dashActiveTimer >= dashActiveMaxTime || (dashReloadTimer > 0 && !firstTimeInDashLoop) || player.WallCheck.RightWallCheck())
            if (!canMove || dashActiveTimer >= dashActiveMaxTime || (dashReloadTimer < dashReloadMaxTime && !firstTimeInDashLoop) || player.WallCheck.RightWallCheck())
                inDash = false;
            if (inDash)
            {
                if (!airDash)
                {
                    if (player.InGround || GetWallSliding)
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
                    // Debug.Log("Dash here time: " + dashReloadMaxTime);
                    // rb.AddForceX(direction.x * DashSpeed.x,ForceMode2D.Impulse);
                    player.RigibodyGravityScale = 0;
                    firstTimeInDashLoop = true;
                }
                //dashReloadTimer = dashReloadMaxTime;
                dashReloadTimer = 0;
                dashActiveTimer += Time.deltaTime;
            }
            if (!inDash)
            {
                DashSpeed = new Vector2(1, 0);
                if (player.gravityDirection > 0)
                    player.RigibodyGravityScale = -player.gravityScale;
                else
                    player.RigibodyGravityScale = player.gravityScale;
                //dashReloadTimer -= Time.deltaTime;
                dashReloadTimer += Time.deltaTime;
                dashActiveTimer -= Time.deltaTime;
                //if (dashReloadTimer < 0)
                //    dashReloadTimer = 0;
                if (dashActiveTimer < 0)
                    dashActiveTimer = 0;
                firstTimeInDashLoop = false;
            }
           
        }
        private void DashBarController()
        {
            if (firstTimeInDashLoop && inDash)
            {
                if(dashBar==null)
                    dashBar = player.CreateBar(Color.blue, dashReloadMaxTime);
                IBarUI c = dashBar.GetComponent<IBarUI>();
                c.Value = 0;
                c.MaxValue = dashReloadMaxTime;
             
            }
            if(!inDash)
            {
                if (dashBar != null)
                {
                    Debug.Log("Reload Timer: " + dashReloadMaxTime);
                    if (dashReloadTimer>=dashReloadMaxTime)
                        Destroy(dashBar);
                    else
                    {
                        IBarUI c = dashBar.GetComponent<IBarUI>();
                        c.Value = dashReloadTimer;
                    }
                }
            }
        }
        public void FreezePlayerMove()
        {
            direction.x = 0;
            player.RigibodyBodyType = RigidbodyType2D.Static;
            player.RigibodyLinearVelocity = Vector2.zero;
            player.MoveInputOn = false;
            anim.SetFloat(WalkID, 0);
            //anim.SetBool(WalkID, false);
        }
        public void UnFreezePlayerMove()
        {
            player.RigibodyBodyType = RigidbodyType2D.Dynamic;
            player.MoveInputOn = true;
        }
        private void WallSliding()
        {
            anim.SetBool(WallJumpID, GetWallSliding);
            if (GetWallSliding)
            {
                player.RigibodyLinearVelocityY = wallSlideSpeed;
                TimeInFastFall = 0;
                if (canWallJump)
                {
                    player.RigibodyLinearVelocity = Vector2.zero;
                    player.AddForce(new Vector2(wallJumpXForce * direction.x * -1, wallJumpYForce), ForceMode2D.Impulse, 0.2f);

                }
            }
            else
                canWallJump = false;

        }

        public void JumpDownEffect()
        {
            var bounce = new BounceEffect(anim.gameObject.transform);
            EffectManager.instance.AddEffect(bounce);
            JumpDownParticles.Play();

        }


        public void GravityChanged()
        {
            jumpForce *= -1;
            wallJumpYForce *= -1;
        }

        internal void HandlerHability()
        {
            if (!player.InGround || !GameManager.Instance.PlayerStates.HasGravity)
                return;
            player.ChangeGravity(player.gravityDirection * -1);
        }
    }
}
