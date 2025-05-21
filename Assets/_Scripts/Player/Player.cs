using System;
using System.Collections;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Items;
using UnityEngine;
using UnityEngine.InputSystem;
namespace br.com.bonus630.thefrog.Player
{

    public class Player : MonoBehaviour, IPlayer
    {
        [SerializeField] private GameObject footer;
        [SerializeField] private GameObject projectile;
        [SerializeField] private GameObject fireball;
        [SerializeField] private Transform projectilesSpawPoint;
        public int CurrentLife { get { return playerHealth.CurrentLife; } set { playerHealth.CurrentLife = value; } }
        [Header("Sounds")]
        [SerializeField] private AudioClip throwProjectileSFX;
        [SerializeField] private AudioClip Entrace;
        [Header("Effects")]
        [SerializeField] private ParticleSystem GravityParticles;



        public PlayerDialogue playerDialogue { get; private set; }
        public PlayerHealth playerHealth { get; private set; }
        public PlayerMovement playerMovement { get; private set; }

        [Header("Others")]
        private Rigidbody2D rb;
        private Animator anim;
        public WallCheck WallCheck { get; private set; }
        private BoxCollider2D footerCollider;
        private AudioSource audioSource;
        public float gravityDirection = 1;
        public float gravityScale = 4f;
        public bool knockUp { get; set; } = false;
        [SerializeField] public Vector2 knockUpForce;


        public bool InGround { get; set; }


        public bool InputsOn { get; set; } = true;
        //private bool isStartJumpTimer;


        //private float jumpTimeCharger;

        //private bool teste;





        public float LookFor { get; set; } = 1;

        /// <summary>
        /// private PlayerStates states;
        /// </summary>
        public GameObject FooterColliding { get { return footer; } protected set { footer = value; } }
        public bool InputOn { get { return InputsOn; } set { InputsOn = value; } }

        public float Speed { get { return playerMovement.Speed; } set { playerMovement.Speed = value; } }
        public float JumpForce { get { return playerMovement.JumpForce; } set { playerMovement.JumpForce = value; } }

        void Awake()
        {

            GetComponents();
            //jumpTimeCharger = startJumpTime;
        }
        private void GetComponents()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            footerCollider = footer.GetComponent<BoxCollider2D>();
            WallCheck = GetComponent<WallCheck>();
            playerDialogue = GetComponent<PlayerDialogue>();
            playerHealth = GetComponent<PlayerHealth>();
            playerMovement = GetComponent<PlayerMovement>();
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
        public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse, float time = 1f)
        {
            StartCoroutine(RemoveInputs(time));
            rb.AddForce(force, mode);
        }
        void FixedUpdate()
        {

        }

        private void Update()
        {

            //RaycastHit2D hit2D = Physics2D.BoxCast(Vector2.zero, new Vector2(0.140f, 0.01f), 0, Vector2.down);
            //Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.140f, 0.01f));

#if UNITY_EDITOR
            if (Input.GetKeyUp(KeyCode.W))
            {
                //MusicSource m = FindAnyObjectByType<MusicSource>();
                //m.CrossFade(BackgroundMusic.AppleTree);
                // GameObject.Find("Virtual Camera").GetComponent<Animator>().SetTrigger("Shake");
                //GameManager.Instance.UpdatePlayer();
                GameObject.FindAnyObjectByType<CamerasController>().ShakeCameraEffect();
            }

#endif

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
            playerMovement.HandlerMove(context);
        }
        public void OnDash(InputAction.CallbackContext context)
        {
            playerMovement.HandlerDash(context);
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            playerMovement.HandlerJump(context);
        }
        public void OnAttack(InputAction.CallbackContext context)
        {
            playerDialogue.OnAttack(context);
        }
        public void OnSpirit(InputAction.CallbackContext context)
        {
            if (GameManager.Instance.PlayerStates.HasFireball)
                LaunchSpirit();
        }
        public void OnHability(InputAction.CallbackContext context)
        {
            if (context.started)
                if (InGround && GameManager.Instance.PlayerStates.HasGravity)
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

        public void ChangeGravity(float gravityDirection, float speed = 0.05f)
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

                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
                rb.gravityScale *= -1;
                knockUpForce *= -1;
                playerMovement.GravityChanged();
            }

        }
        private IEnumerator ChangeGravityIenumerator(float speed)
        {
            yield return new WaitForSeconds(speed);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
            var m = GravityParticles.main;
            m.gravityModifierMultiplier = -1;
            playerMovement.GravityChanged();
            rb.gravityScale *= -1;

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


        public IEnumerator RemoveInputs(float time = 0.2f)
        {
            InputsOn = false;
            yield return new WaitForSeconds(time);
            InputsOn = true;
        }
        public void KnockedUp()
        {
            if (knockUp)
            {
                Debug.Log("knocked: " + knockUpForce.y);
                if (rb == null)
                {
                    Debug.LogError("Rigidbody2D está null no Build!");
                    return;
                }
                rb.AddForce(knockUpForce, ForceMode2D.Impulse);
                knockUp = false;
            }
        }
        private void Die()
        {
            playerHealth.Die();
        }
        public void Hit()
        {
            playerHealth.Hit();
        }
        public void GameOver()
        {
            playerHealth.GameOver();
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


        public void ReadDialogue()
        {
            playerDialogue.ReadDialogue();
        }
    }
}
