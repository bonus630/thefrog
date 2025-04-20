using System.Collections;
using UnityEngine;
using br.com.bonus630.thefrog.DialogueSystem;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Items;
using UnityEngine.InputSystem.LowLevel;
using System.Threading;
using UnityEngine.InputSystem;
using System;
namespace br.com.bonus630.thefrog.Caracters
{

    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float jumpForce;
        [SerializeField] private GameObject footer;
        [SerializeField] float LinearMaxY = 15;
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject fireball;
        [SerializeField] private Transform projectilesSpawPoint;
        [Header("Sounds")]
        [SerializeField] private AudioClip jumpSFX;
        [SerializeField] private AudioClip hitSFX;
        [SerializeField] private AudioClip throwProjectileSFX;
        [SerializeField] private AudioClip Entrace;
        [Header("Effects")]
        [SerializeField] private ParticleSystem JumpDownParticles;
        [SerializeField] private ParticleSystem GravityParticles;
        //[Header("Inputs")]
        //[SerializeField] private InputAction moveAction;

        private Rigidbody2D rb;
        private Animator anim;
        private WallCheck wallCheck;
        private BoxCollider2D footerCollider;
        private AudioSource audioSource;
        private br.com.bonus630.thefrog.DialogueSystem.DialogueSystem dialogueSystem;

        private int jumps = 2;
        [field: SerializeField] public int CurrentLife { get; set; } = 2;
        private float doubleJumpForce;
        private Vector2 direction;
        private float invencibleTimer = 1.2f;
        public float gravityDirection = 1;
        private float timeInFastFall = 0;

        public bool inGround;
        private float acceleration = 0;
        private bool isJumping;
        private bool doubleJump;
        private bool readyToJump;
        private bool invencible;
        private bool knockUp = false;
        [SerializeField] private Vector2 knockUpForce;
        public bool InputsOn { get; set; } = true;
        //private bool isStartJumpTimer;

        private readonly float invencibleTime = 1.2f;
        //private float jumpTimeCharger;

        //[Header("Wall")]
        //private bool teste;
        private bool isWallSliding;
        private bool canWallJump;

        private readonly float wallSlideSpeed = -0.36f;
        private readonly float wallJumpXForce = 120f;
        private  float wallJumpYForce = 220f;
        private readonly float maxTimeInFall = 0.6f;


        protected readonly int HitID = Animator.StringToHash("Hit");
        protected readonly int WalkID = Animator.StringToHash("Walk");
        protected readonly int RunID = Animator.StringToHash("Run");
        protected readonly int JumpID = Animator.StringToHash("Jump");
        protected readonly int WallJumpID = Animator.StringToHash("WallJump");
        protected readonly int DoubleJumpID = Animator.StringToHash("DoubleJump");
        protected readonly int LifeID = Animator.StringToHash("Life");

        public float LookFor { get; private set; } = 1;
        public float Speed { get { return speed; } set { speed = value; } }
        public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }
        /// <summary>
        /// private PlayerStates states;
        /// </summary>
        private Transform npc;
        private IInteract interacting;
        public GameObject FooterColliding { get; protected set; }
        public bool InputOn { get { return InputsOn; } set { InputsOn = value; } }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            CurrentLife = GameManager.Instance.PlayerStates.Hearts;
            Speed = GameManager.Instance.PlayerStates.Speed;
            jumpForce = GameManager.Instance.PlayerStates.JumpForce;
            //Debug
#if UNITY_EDITOR
            CurrentLife = 100;
#endif
            rb = GetComponent<Rigidbody2D>();

            anim = GetComponent<Animator>();
            footerCollider = footer.GetComponent<BoxCollider2D>();
            wallCheck = GetComponent<WallCheck>();
            audioSource = GetComponent<AudioSource>();
            dialogueSystem = FindAnyObjectByType<br.com.bonus630.thefrog.DialogueSystem.DialogueSystem>();
            //jumpTimeCharger = startJumpTime;
        }
        private void Start()
        {
            //states = GameManager.Instance.PlayerStates;
            //Debug
#if !UNITY_EDITOR
            transform.position = GameManager.Instance.PlayerStates.PlayerPosition.Position;
            if (transform.position == GameObject.Find(GameManager.Instance.StartPointBuilder).gameObject.transform.position)
            {
                audioSource.PlayOneShot(Entrace);
                //rb.AddForce(new Vector2(100, 480), ForceMode2D.Impulse);
                AddForce(new Vector2(100, 480), ForceMode2D.Impulse);
            }
#endif
        }
        public void AddForce(Vector2 force,ForceMode2D mode = ForceMode2D.Impulse, float time = 1f)
        {
            StartCoroutine(RemoveInputs(time));
            rb.AddForce(force,mode);
        }
        
        void FixedUpdate()
        {
            //       Debug.DrawLine(transform.position, Vector3.up * gravityDirection * 10);
            if (Mathf.Abs(rb.linearVelocityY * gravityDirection) > Mathf.Abs(LinearMaxY))
            {
                timeInFastFall += Time.deltaTime;
                if (GameManager.Instance.PlayerStates.FallsControl && Input.GetButtonDown("Jump"))
                {
                    timeInFastFall = 0;
                    // Physics2D.Raycast(transform.position, Vector2.up * gravityDirection);
                    rb.linearVelocityY = LinearMaxY * gravityDirection;
                    JumpDownEffect();
                }
                if (timeInFastFall > maxTimeInFall)
                    Die();
            }
            if (InputsOn)
                Move();
            Jump();
            if (GameManager.Instance.PlayerStates.HasDoubleJump)
                DoubleJump();
            KnockedUp();
            if (GameManager.Instance.PlayerStates.HasWallJump)
                WallSliding();
            if (invencible)
            {
                invencibleTimer -= Time.deltaTime;
                if (invencibleTimer < 0)
                {
                    invencible = false;
                }
            }
            else
            {
                invencibleTimer = invencibleTime;
            }


            //FooterColliding = Physics2D.Linecast(r.transform.position, l.transform.position) && isJumping;
            //Debug.Log(FooterColliding);
        }

        private void Update()
        {

            //Debug.Log("---------------wall check -------------------");
            //Debug.Log("inground: " + inGround);
            //Debug.Log("linearVelocityY: " + rb.linearVelocityY);
            //Debug.Log("direction.x: " + direction.x);
            //Debug.Log("wallCheck.RightWallCheck(): " + wallCheck.RightWallCheck());
            //Debug.Log("---------------wall check end -------------------");


            //isWallSliding = !inGround && rb.linearVelocityY < 0 && Mathf.Abs(direction.x) > 0 && wallCheck.RightWallCheck();
             isWallSliding = IsWallSliding();
            //Debug.Log("iswallSliding:" + isWallSliding);
            if (wallCheck.CheckGround())
            {
                if (!inGround)
                    resetJump();
            }
            else
            {
                inGround = false;
                anim.SetBool(JumpID, true);
            }
            //RaycastHit2D hit2D = Physics2D.BoxCast(Vector2.zero, new Vector2(0.140f, 0.01f), 0, Vector2.down);
            //Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.140f, 0.01f));

            if (interacting != null)
                interacting.ReadyToInteract(Mathf.Abs(transform.position.x - interacting.GetTransform().position.x) < 1.1f && wallCheck.IsFaceTo(interacting.GetTransform()));
#if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.W))
            {
                //MusicSource m = FindAnyObjectByType<MusicSource>();
                //m.CrossFade(BackgroundMusic.AppleTree);
                GameObject.Find("Virtual Camera").GetComponent<Animator>().SetTrigger("Shake");
                //GameManager.Instance.UpdatePlayer();
            }

#endif

        }
        private bool IsWallSliding()
        {
            bool falling = false;
            if(gravityDirection == 1 && rb.linearVelocityY > 0)
                falling = true;
            if(gravityDirection == -1 && rb.linearVelocityY < 0)
                falling = true;
            if (!falling)
                return false;
            return !inGround  && Mathf.Abs(direction.x) > 0 && wallCheck.RightWallCheck();

        }
        //private bool CheckGround()
        //{
        //    RaycastHit2D raycastHit2D = Physics2D.Raycast(Vector2.zero, Vector2.down, 0.5f, LayerMask.GetMask(new string[] { "Ground", "Platform", "StaticPlatforms" }));
        //    Gizmos.DrawLine(Vector2.zero,new Vector2.down, 0.5f)
        //}
        private GameObject currentBullet;
        private float nextLaunch = 0f;
        private void SelectProjectilie()
        {
            currentBullet = fireball;
        }

        private void LaunchSpirit()
        {
            if (Time.time > nextLaunch)
            {
               // GameObject bullet = Instantiate(fireball, new Vector2(rb.position.x + (0.8f * LookFor), rb.position.y - 0.07f), Quaternion.Euler(0, LookFor > 0 ? 0 : -180, 0));
                GameObject bullet = Instantiate(fireball, projectilesSpawPoint.position, Quaternion.Euler(0, LookFor > 0 ? 0 : -180, 0));
                if (bullet != null && bullet.TryGetComponent<IProjectilies>(out IProjectilies projectilie))
                {
                    projectilie.Launch(new Vector2(LookFor, 0));
                    nextLaunch = Time.time + projectilie.ReloadTime();
                }
            }
        }
        private void CheckFall()
        {

        }
        public void OnMove(InputAction.CallbackContext context)
        {
            direction = context.ReadValue<Vector2>();
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                if (inGround)
                {
                    isJumping = true;
                    jumps--;
                }
                if (knockUp)
                {
                    knockUpForce *= 10;
                }
                if (isWallSliding)
                {
                    canWallJump = true;
                }
                if (doubleJump)
                {
                    readyToJump = true;
                }
            }
            //if (context.performed)
            //    Debug.Log("Jump context perfomed"); 
            if (context.canceled)
            {
                if (rb.linearVelocityY > 0)
                {
                    rb.linearVelocityY *= 0.2f * gravityDirection;
                    doubleJump = true;
                    jumps--;
                }
            }
        }
        public void OnAttack(InputAction.CallbackContext context)
        {

            if (context.performed)
            {

                if (npc != null && Mathf.Abs(transform.position.x - npc.position.x) < 1.1f && wallCheck.IsFaceTo(npc))
                {

                    if (interacting is INPC inpc)
                    {

                        if (inpc.HaveMoreDialogue())
                        {

                            //vou fazer um teste, se bugar é aqui o erro
                            dialogueSystem.DialogueData = inpc.CurrentDialogueData;
                            Debug.Log(inpc.CurrentDialogueData.name);
                            dialogueSystem.DialogueVariables = inpc.GetDialogueVariables();
                            dialogueSystem.Next();
                        }
                        else
                        {

                            inpc.SetFinishDialogue();
                        }
                    }


                }
                else if (interacting != null && Mathf.Abs(transform.position.x - interacting.GetTransform().position.x) < 1.1f && wallCheck.IsFaceTo(interacting.GetTransform()))
                {
                    interacting.Interact();
                }
                else if (tips != null)
                {

                    ReadDialogue();
                }
                else
                {

                    Launch();
                }
            }
        }
        public void OnSpirit(InputAction.CallbackContext context)
        {
            if (GameManager.Instance.PlayerStates.HasFireball)
                LaunchSpirit();
        }
        public void OnHability(InputAction.CallbackContext context)
        {
            if (context.started)
                if (inGround && GameManager.Instance.PlayerStates.HasGravity)
                    ChangeGravity(this.gravityDirection * -1);
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            //if (collision.gameObject.layer == 8 || collision.gameObject.layer == 17)
            //{
            //    if (FooterTouching(collision.collider))
            //        //Debug.Log("Reset jump");
            //        resetJump();
            //}
            if (collision.gameObject.layer == 13)
            {
                if (FooterTouching(collision.collider))
                {
                    gameObject.transform.parent = collision.transform;
                    //resetJump();
                }
            }
            if (collision.gameObject.layer == 10)
                Die();
            if (collision.gameObject.layer == 6)
            {
                if (!FooterTouching(collision.collider))
                {

                    Hit();
                    knockUp = true;
                    knockUpForce = collision.GetContact(0).normal * 40 * -1;
                }
            }

        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            //if (collision.gameObject.layer == 8 || collision.gameObject.layer == 13 || collision.gameObject.layer == 17)
            //{
            //    if (!FooterTouching(collision.collider))
            //    {
            //        inGround = false;
            //        anim.SetBool(JumpID, true);
            //    }
            //}
            if (collision.gameObject.layer == 13)
            {
                gameObject.transform.parent = null;
            }

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("NPC"))
            {
                //interacting = npc.gameObject.GetComponent<IInteract>();
                //Debug.Log("NPC trigger enter");
                npc = collision.transform;
                NPC_WallJump_Tutorial npcScript;
                if (npc.gameObject.TryGetComponent<NPC_WallJump_Tutorial>(out npcScript))
                {
                    interacting = npcScript as IInteract;
                    // npcScript.SetEvent(FindAnyObjectByType<EventsManager>().CurrentEvent());
                    dialogueSystem.DialogueData = npcScript.CurrentDialogueData;
                }
                NPCBase npcScript2;
                if (npc.gameObject.TryGetComponent<NPCBase>(out npcScript2))
                {
                    dialogueSystem.DialogueData = npcScript2.CurrentDialogueData;
                    interacting = npcScript2 as IInteract;
                }
            }
            if (collision.gameObject.CompareTag("Item"))
            {
                collision.gameObject.TryGetComponent<IInteract>(out interacting);
                Debug.Log("Item trigger enter:" + interacting);
            }
            if (collision.gameObject.CompareTag("Tips"))
            {

                tips = collision.gameObject.GetComponent<ITips>();
                dialogueSystem.DialogueData = tips.GetDialogue();
                tips.AutoPlayer(gameObject);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("NPC"))
            {
                Debug.Log("NPC trigger exit");
                npc = null;
                interacting = null;
                dialogueSystem.ResetDialog();
            }
            if (collision.gameObject.CompareTag("Item"))
            {
                Debug.Log("Item trigger exit");
                interacting = null;
            }
            if (collision.gameObject.CompareTag("Tips"))
            {
                //  Debug.Log("tips trigger exit");
                tips = null;
                dialogueSystem.ResetDialog();
            }
        }
        public void ReadDialogue()
        {
            dialogueSystem.Next();
        }
        public void SetDialogue(DialogueData dialogue)
        {
            dialogueSystem.DialogueData = dialogue;
        }
        public void ResetDialog()
        {
            dialogueSystem.ResetDialog();
        }
        private ITips tips = null;
        public void ChangeGravity(float gravityDirection, float speed = 0.4f)
        {
            this.gravityDirection = gravityDirection;
            //LinearMaxY *= -1;
            GameManager.Instance.ActiveSkill(this.gravityDirection > 0);
            if (this.gravityDirection > 0)
            {
                Debug.Log(gravityDirection);
                var m = GravityParticles.main;
                m.gravityModifierMultiplier = 0;
                GravityParticles.Play();
                StartCoroutine(ChangeGravityIenumerator(speed));
            }
            else
            {
                Debug.Log(gravityDirection);
                GravityParticles.Stop();
                jumpForce *= -1;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
                rb.gravityScale *= -1;
                knockUpForce *= -1;
                wallJumpYForce *= -1;
            }

        }
        private IEnumerator ChangeGravityIenumerator(float speed)
        {
            yield return new WaitForSeconds(speed);
            var m = GravityParticles.main;
            m.gravityModifierMultiplier = -1;
            jumpForce *= -1;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
            rb.gravityScale *= -1;
            wallJumpYForce *= -1;
        }
        public void Alert()
        {
            transform.GetChild(4).gameObject.SetActive(true);
            Invoke(nameof(Dealert), 2f);
        }
        private void Dealert()
        {
            transform.GetChild(4).gameObject.SetActive(false);
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


            //if(isStartJumpTimer)
            //{
            //    jumpTimeCharger += Time.deltaTime;

            //}
            //if(Input.GetButtonDown("Jump"))
            //{
            //    isStartJumpTimer = true;


            //}
            //if (Input.GetButtonUp("Jump") || jumpTimeCharger >= 1)
            //{
            //    isStartJumpTimer = false;
            //   readyToJump = true;

            //}
            //if(readyToJump)
            //{

            //    if (!isJumping)
            //    {

            //        rg.AddForce(new Vector2(0, jumpForce*jumpTimeCharger), ForceMode2D.Impulse);
            //        doubleJump = true;

            //    }
            //    else
            //    {

            //        if (doubleJump)
            //        {
            //            rg.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            //            doubleJump = false;
            //            anim.SetTrigger("DoubleJump");
            //        }
            //    }
            //    readyToJump = false;
            //    jumpTimeCharger = startJumpTime;
            //}
        }
        private void resetJump()
        {

            JumpDownEffect();
            inGround = true;
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
                LookFor = direction.x < 0 ? -1 : 1;

                if (direction.x > 0)
                {
                    if (transform.localScale.x < 0)
                        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    if (wallCheck.RightWallCheck())
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
                        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
                    if (wallCheck.RightWallCheck())
                        canMove = false;
                    else
                    {
                        acceleration -= 0.4f;
                        if (acceleration < -speed)
                            acceleration = -speed;
                    }

                }


            }
            if (canMove)
            {
                anim.SetBool(WalkID, true);
                //Vector3 moviment = new Vector3(direction, 0, 0);
                //transform.position += moviment * Time.deltaTime * speed;
                //if(Mathf.Abs(rb.linearVelocityX) < speed / 2)
                //   rb.AddForceX(speed * 10 * direction.x,ForceMode2D.Force);
                //else 
                // rb.linearVelocityX = speed * direction.x;
                if (inGround)
                    rb.linearVelocityX = acceleration;
                else
                    rb.linearVelocityX = speed * direction.x;

            }
            else
            {
                // if (inGround)
                //  {
                anim.SetBool(WalkID, false);
                //   }
                rb.linearVelocityX = 0;
                acceleration = 0f;
            }
        }
        public void Launch()
        {
            if (GameManager.Instance.PlayerStates.Shurykens > 0)
            {
               // GameObject projectileGO = Instantiate(projectile, new Vector2(rb.position.x + (0.12f * LookFor), rb.position.y - 0.07f), Quaternion.identity);
                GameObject projectileGO = Instantiate(projectile, projectilesSpawPoint.position, Quaternion.identity);
                Shuryken projectileScript = projectileGO.GetComponent<Shuryken>();
                projectileScript.Launch(LookFor, 10f);
                //animator.SetTrigger(launchHash);
                audioSource.PlayOneShot(throwProjectileSFX);
                ChangeNumberShurykens(-1);
            }
        }
        public void FreezePlayerMove()
        {
            direction.x = 0;
            rb.linearVelocity = Vector2.zero;
            InputsOn = false;
            anim.SetBool(WalkID, false);
        }
        public void UnFreezePlayerMove()
        {
            InputsOn = true;
        }
        private void WallSliding()
        {
            anim.SetBool(WallJumpID, isWallSliding);
            // Debug.Log("IsWallSliding: " + isWallSliding);
            if (isWallSliding)
            {
                rb.linearVelocityY = wallSlideSpeed;
                if (canWallJump)
                {
                    rb.linearVelocity = Vector2.zero;
                    rb.AddForce(new Vector2(wallJumpXForce * direction.x * -1, wallJumpYForce), ForceMode2D.Impulse);
                    StartCoroutine(RemoveInputs());
                }
            }
            else
                canWallJump = false;

        }
        private IEnumerator RemoveInputs(float time = 0.2f)
        {
            InputsOn = false;
            yield return new WaitForSeconds(time);
            InputsOn = true;
        }
        private void KnockedUp()
        {
            if (knockUp)
            {
                //Debug.Log("knocked: " + knockUpForce.y);
                rb.AddForce(knockUpForce, ForceMode2D.Impulse);
                knockUp = false;
            }
        }
        private void Die()
        {
            FreezePlayerMove();
            CurrentLife = 0;
            Hit();
        }
        public void Hit()
        {
            if (invencible)
                return;
            GameManager.Instance.UpdateHeart(-1);
            invencible = true;
            anim.SetTrigger(HitID);
            audioSource.PlayOneShot(hitSFX);
            anim.SetInteger(LifeID, CurrentLife);

            //if (life <= 0)
            //    Invoke(nameof(GameOver), 0.24f);

        }
        public void GameOver()
        {
            InputsOn = false;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.bodyType = RigidbodyType2D.Static;
            footerCollider.isTrigger = true;
            GameManager.Instance.GameOver();
        }
        public bool FooterTouching(Collider2D collision)
        {
            return footerCollider.IsTouching(collision);
        }
        public void KnockUp(Vector2 force)
        {
            //Debug.Log("knock jump: " + Input.GetButtonDown("Jump"));
            knockUp = true;
            if (Input.GetButtonDown("Jump"))
                force *= 2;
            knockUpForce = force * 2;
        }

        public void ChangeNumberShurykens(int shurykens)
        {
            GameManager.Instance.PlayerStates.Shurykens += shurykens;
            //GameManager.Instance.Shurykens = states.Shurykens;
            GameManager.Instance.UpdateShurykens();
        }
        // float prevRigidibodyVelocity = 0;
        // bool landing = false;
        public void JumpDownEffect()
        {
            JumpDownParticles.Play();
        }
    }
}
