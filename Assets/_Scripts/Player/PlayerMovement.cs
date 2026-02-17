using System;
using System.Collections;
using br.com.bonus630.thefrog.Debuggers;
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
        [SerializeField] private float wallJumpRemoveInputTime = 0.2f;
        [SerializeField] private float LinearMaxY = 15;
        [SerializeField] float dashActiveMaxTime = 0.5f;
        [SerializeField] float dashReloadMaxTime = 0.5f;
        [SerializeField] private AudioClip jumpSFX;
        [SerializeField] private ParticleSystem JumpDownParticles;
        [SerializeField] private ParticleSystem DashParticles;
        [SerializeField] private ParticleSystem FastFallParticles;
        [SerializeField] private SpriteRenderer playerSpriteRender;
        private SpriteAfterImageEffect spriteAfterImage;
        float dashActiveTimer = 0;
        float dashReloadTimer = 0;
        float coyouteTime = 0.2f;
        float coyouteTimer = 0;
        float jumpBufferTime = 0.2f;
        float jumpBufferTimer = 0;
        // public float Speed { get { return speed; } set { speed = value; } }
        // public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }
        public float TimeInFastFall { get; set; } = 0;
        public bool UseYvelocityLimit { get; set; } = true;
        public bool IgnoreDamping { get; set; } = false;

        private int jumps = 2;
        private float doubleJumpForce;
        public Vector2 direction;
        private Vector2 DashSpeed = new Vector2(1, 0);
        private float acceleration = 0;
        private bool isJumping;
        private bool doubleJump;
        private bool readyToJump;
        private bool resetFastFall = false; //quando verdadeiro ativa o FallControl
        private bool jumpReleasedAfterFall = false; //flag auxiliar para o FallControl
        public bool GetWallSliding { get; private set; }
        private bool canWallJump;
        bool inDash = false;
        public bool airDash = false;
        bool dashInitialized = false;
        //  bool HasDoubleJump = false;
        // bool HasWallJump = false;

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
            // Speed = player.playerManager.PlayerStates.Speed;
            //  jumpForce = player.playerManager.PlayerStates.JumpForce;
            dashReloadTimer = dashReloadMaxTime;
            //  HasDoubleJump = player.playerManager.PlayerStates.HasDoubleJump;
            //  HasWallJump = player.playerManager.PlayerStates.HasWallJump;
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
            else if (!GetWallSliding)
            {
                coyouteTimer -= Time.deltaTime;
                anim.SetBool(FallingID, (IsFalling() && !player.playerFallControl.InFallControl));
            }
            DashBarController();
            //   Debug.Log($"[PlayerMovement]DashReloadTime: {dashReloadTimer} inDash: {inDash}");

        }
        void FixedUpdate()
        {
            //if (player.knockUp)
            //{
            //    player.KnockedUpOnHit();
            //    airDash = false;
            //}
            //Debug.Log("[playermovement] update knockup force:" + player.knockUpForce);
            // Debug.Log("[PlayerMovement] TimeInFastFall:" + TimeInFastFall);

            player.ApplyKnockUp();
            if (Mathf.Abs(player.RigibodyLinearVelocity.y * player.gravityDirection) > Mathf.Abs(LinearMaxY))
            {
                TimeInFastFall += Time.deltaTime;
                if (resetFastFall)
                {
                    FallsControl();
                }
                if (UseYvelocityLimit)
                {
                    Vector2 velocity = player.RigibodyLinearVelocity;
                    velocity.y = (Mathf.Abs(LinearMaxY) + 0.1f) * player.gravityDirection;
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
            player.playerHealth.PrepareFallDie = TimeInFastFall > maxTimeInFall;
            if (player.MoveInputOn)
                Move();
            Jump();

            if (player.playerManager.PlayerStates.HasDoubleJump)
                DoubleJump();
            if (player.playerManager.PlayerStates.HasWallJump)
                WallSliding();
        }

        public void FallsControl()
        {
            resetFastFall = false;
            player.playerHealth.PrepareFallDie = false;
            // Debug.Log("Playermovement fallscontrol");
            player.playerFallControl.FallsControl(true);

        }
        private bool IsWallSliding()
        {
            bool falling = IsFalling();
            bool prepareWall = falling && Mathf.Abs(direction.x) > 0 && player.WallCheck.RightWallCheck();
            if (prepareWall)
            {
                //Debug.Log("Player graviti:" + player.gravityDirection);
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
                if (player.playerManager.PlayerStates.FallsControl && TimeInFastFall > 0 && jumpReleasedAfterFall)
                {
                    resetFastFall = true;
                    jumpReleasedAfterFall = false;
                    //Debug.Log("[PlayerMovement] resetFastFall fallcontrol");
                }
            }
            //if (context.performed)
            //    Debug.Log("Jump context perfomed"); 
            if (context.canceled)
            {
                if (!player.knockUp && !IgnoreDamping)
                {
                    if ((player.gravityDirection.Equals((float)PlayerGravityDirection.DOWN) && player.RigibodyLinearVelocity.y > 0) ||
                        (player.gravityDirection.Equals((float)PlayerGravityDirection.UP) && player.RigibodyLinearVelocity.y < 0))
                    {
                        player.RigibodyLinearVelocityY *= 0.2f;
                        doubleJump = true;
                        jumps--;
                    }
                }
                if (player.playerManager.PlayerStates.FallsControl)
                {
                    jumpReleasedAfterFall = true;
                }
            }
        }
        public void HandlerMove(Vector2 direction)
        {
            this.direction = direction;

        }
        public void HandlerDash(InputAction.CallbackContext context)
        {
            if (player.playerManager.PlayerStates.HasDash)
            {
                if (context.started)
                    inDash = true;
                if (context.canceled)
                    inDash = false;
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
#if UNITY_EDITOR
        float distance = 0f;
#endif
        private void Jump()
        {
#if UNITY_EDITOR
            distance = player.transform.position.x;
#endif
            if (isJumping && coyouteTimer > 0)
            {
                player.RigibodyLinearVelocityY = jumpForce;
                audioSource.PlayOneShot(jumpSFX);
                isJumping = false;
            }
        }
        private void resetJump()
        {
#if UNITY_EDITOR
            distance = player.transform.position.x - distance;
            //  Debug.Log("Jump Distance:" + distance);
#endif
            JumpDownEffect();
            player.InGround = true;
            IgnoreDamping = false;
            airDash = false;
            doubleJump = false;
            anim.SetBool(JumpID, false);
            jumps = 2;
            Invoke(nameof(resetTimeInFastFall), 0.1f);
            player.playerFallControl.FallsControl(false);
            anim.SetBool(FallingID, false);
        }
        private void resetTimeInFastFall() => TimeInFastFall = 0;
        private void Move()
        {
            // Debug.Log("[PlayerMovement] ignoreDamping:" + IgnoreDamping);
            // Watcher.Watch(IgnoreDamping,this,nameof(IgnoreDamping));
            //if(IgnoreDamping)
            //    return;

            bool canMove = true;
            if (direction.x == 0 && !IgnoreDamping)
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

                //vamos tentr um hack para funcionar isso
                if (IgnoreDamping && player.playerHealth.InHit)
                {
                    return;
                    acceleration = Mathf.Sign(player.RigibodyLinearVelocityX);
                    //    //return;
                    //   // Debug.Log("acceleration:" + acceleration);
                    Debug.Log("[PlayerMovement] rigidibodyVelocityX:" + player.RigibodyLinearVelocityX);
                    // Debug.Break();
                }

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
            if (IgnoreDamping)
            {
                //    acceleration = 0.2f * Mathf.Sign(player.RigibodyLinearVelocityX);
                //    //return;
                //   // Debug.Log("acceleration:" + acceleration);
                // Debug.Log("[PlayerMovement] rigidibodyVelocityX:" + player.RigibodyLinearVelocityX);
                //Debug.Break();
            }
            anim.SetFloat(WalkID, Mathf.Abs(player.RigibodyLinearVelocityX));
        }
        GameObject dashBar;
        //private void Dash(bool canMove)
        //{
        //    if (inDash)
        //    {
        //        TryStartOrUpdateDash(canMove);
        //    }
        //    else
        //    {
        //        UpdateDashCooldown();
        //    }
        //}

        //private void TryStartOrUpdateDash(bool canMove)
        //{
        //    // Se ainda não inicializou, significa que está tentando começar
        //    if (!dashInitialized)
        //    {
        //        // Validação real acontece aqui
        //        if (!CanStartDash(canMove))
        //        {
        //            inDash = false; // Rejeita a tentativa
        //            return;
        //        }

        //        InitializeDash();
        //    }

        //    UpdateDashLoop(canMove);
        //}
        //float originalGravity;
        //private void InitializeDash()
        //{
        //    dashInitialized = true;
        //    dashActiveTimer = 0f;
        //    dashReloadTimer = 0f;

        //    originalGravity = player.RigibodyGravityScale;

        //    if (!player.InGround)
        //        player.RigibodyGravityScale = 0f;

        //    DashSpeed = new Vector2(8f, 0f);

        //    ApplyDashEffect();
        //}

        //private void UpdateDashLoop(bool canMove)
        //{
        //    if (!canMove)
        //    {
        //        EndDash();
        //        return;
        //    }

        //    dashActiveTimer += Time.deltaTime;

        //    if (dashActiveTimer >= dashActiveMaxTime)
        //    {
        //        EndDash();
        //    }
        //}

        //private bool CanStartDash(bool canMove)
        //{
        //    if (!canMove)
        //        return false;

        //    if (dashReloadTimer < dashReloadMaxTime)
        //        return false;

        //    if (player.WallCheck.RightWallCheck())
        //        return false;

        //    return true;
        //}

        //private void EndDash()
        //{
        //    inDash = false;
        //    dashInitialized = false;

        //    DashSpeed = Vector2.zero;
        //    player.RigibodyGravityScale = originalGravity;

        //    spriteAfterImage?.Deactivate();
        //}

        //private void UpdateDashCooldown()
        //{
        //    if (dashReloadTimer < dashReloadMaxTime)
        //    {
        //        dashReloadTimer += Time.deltaTime;
        //    }
        //}
        private void Dash(bool canMove)
        {
            if (!CanDash(canMove))
                inDash = false;
            if (inDash)
            {
                if (dashInitialized)
                    DashLoop();
                else
                    InitializeDash();
            }
            else
            {
                EndDash();
            }
        }
        private bool CanDash(bool canMove)
        {
            if (!canMove)
                return false;
            if (dashActiveTimer >= dashActiveMaxTime)
                return false;
            if (dashReloadTimer < dashReloadMaxTime && !dashInitialized)
                return false;
            if (player.WallCheck.RightWallCheck())
                return false;
            return true;
        }
        private void InitializeDash()
        {
            dashInitialized = true;
            ApplyDashEffect();
            DashSpeed = new Vector2(8, 0);
            player.RigibodyGravityScale = 0;
            dashReloadTimer = 0;
        }
        private void EndDash()
        {
            dashInitialized = false;
            spriteAfterImage?.Deactivate();
            DashSpeed = new Vector2(1, 0);
            if (player.gravityDirection > 0)
                player.RigibodyGravityScale = -player.gravityScale;
            else
                player.RigibodyGravityScale = player.gravityScale;
            dashReloadTimer += Time.deltaTime;
            dashActiveTimer -= Time.deltaTime;
            if (dashActiveTimer < 0)
                dashActiveTimer = 0;
        }
        private void DashLoop()
        {
            dashActiveTimer += Time.deltaTime;
        }
        //private void Dash(bool canMove)
        //{
        //    Debug.Log(canMove);
        //    //Debug.Log(dashActiveTimer >= dashActiveMaxTime);
        //    //Debug.Log((dashReloadTimer < dashReloadMaxTime && !firstTimeInDashLoop));
        //    //Debug.Log(player.WallCheck.RightWallCheck());
        //   if (!canMove || dashActiveTimer >= dashActiveMaxTime || dashReloadTimer > dashReloadMaxTime  || player.WallCheck.RightWallCheck())
        //     inDash = false;
        //    Debug.Log(inDash);
        //    if (inDash)
        //    {
        //        if (!airDash)
        //        {
        //            if (player.InGround || GetWallSliding)
        //                airDash = false;
        //            else
        //                airDash = true;
        //            if (!dashInitialized)
        //            {
        //                ApplyDashEffect();
        //            }
        //            DashSpeed = new Vector2(8, 0);
        //            // Debug.Log("Dash here time: " + dashReloadMaxTime);
        //            // rb.AddForceX(direction.x * DashSpeed.x,ForceMode2D.Impulse);
        //            player.RigibodyGravityScale = 0;
        //            dashInitialized = true;
        //        }
        //       // dashReloadTimer = dashReloadMaxTime;
        //        dashReloadTimer = 0;
        //        dashActiveTimer += Time.deltaTime;
        //    }
        //    if (!inDash)
        //    {
        //        spriteAfterImage?.Deactivate();
        //        DashSpeed = new Vector2(1, 0);
        //        if (player.gravityDirection > 0)
        //            player.RigibodyGravityScale = -player.gravityScale;
        //        else
        //            player.RigibodyGravityScale = player.gravityScale;
        //        //dashReloadTimer -= Time.deltaTime;
        //        dashReloadTimer += Time.deltaTime;
        //        dashActiveTimer -= Time.deltaTime;
        //        //if (dashReloadTimer < 0)
        //        //    dashReloadTimer = 0;
        //        if (dashActiveTimer < 0)
        //            dashActiveTimer = 0;

        //        dashInitialized = true;

        //    }

        //}
        //private bool CanDash(bool canMove)
        //{
        //    Debug.Log($"[PlayerMovement][CanDash] dashActiveTimer:{dashActiveTimer} dashReloadTimer:{dashReloadTimer} firstTimeInDashLoop:{firstTimeInDashLoop} RightWallCheck:{player.WallCheck.RightWallCheck()}");
        //    return !(!canMove || dashActiveTimer >= dashActiveMaxTime || (dashReloadTimer < dashReloadMaxTime && !firstTimeInDashLoop) || player.WallCheck.RightWallCheck());
        //}

        ushort effectID = 0;
        public void ApplyDashEffect()
        {
            spriteAfterImage ??= EffectManager.instance.GetEffect<SpriteAfterImageEffect>(effectID) as SpriteAfterImageEffect;
            spriteAfterImage ??= SpriteAfterImageEffect.Create(playerSpriteRender)
                                                        .WithLifeTime(14)
                                                        .WithSpawnInterval(0.012f)
                                                        .WithLifeTime(1f)
                                                        .WithFadeSpeed(0.1f);
            spriteAfterImage.Activate();
            effectID = EffectManager.instance.AddEffect(spriteAfterImage);
        }
        private void DashBarController()
        {
            if (dashInitialized && inDash)
            {
                IBarUI c = null;
                if (dashBar == null)
                {
                    c = player.barManager.CreateBar(Color.blue, dashReloadMaxTime, player.transform, player.gravityDirection);
                    dashBar = c.gameObject;
                }
                else
                    c = dashBar.GetComponent<IBarUI>();
                c.Value = 0;
                c.MaxValue = dashReloadMaxTime;

            }
            if (!inDash)
            {
                if (dashBar != null)
                {
                    //Debug.Log("Reload Timer: " + dashReloadMaxTime);
                    if (dashReloadTimer >= dashReloadMaxTime)
                        dashBar.GetComponent<IBarUI>().DestroyBar();
                    // Destroy(dashBar);
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
            player.RigibodyLinearVelocity = Vector2.zero;
            player.AllInputsOn(false);
            anim.SetFloat(WalkID, 0);
            direction.x = 0;
            player.RigibodyBodyType = RigidbodyType2D.Static;
            //anim.SetBool(WalkID, false);
        }
        public void UnFreezePlayerMove()
        {
            player.RigibodyBodyType = RigidbodyType2D.Dynamic;
            player.AllInputsOn(true);
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
                    player.AddForce(new Vector2(wallJumpXForce * direction.x * -1, wallJumpYForce), ForceMode2D.Impulse, wallJumpRemoveInputTime);

                }
            }
            else
                canWallJump = false;

        }
        public void JumpDownEffect()
        {
            var bounce = BounceEffect.Create(anim.gameObject.transform);
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
            if (!player.InGround || !player.playerManager.PlayerStates.HasGravity)
                return;
            player.ChangeGravity(player.gravityDirection * -1);
        }
    }

}
