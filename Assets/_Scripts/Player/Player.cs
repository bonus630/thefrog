using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using br.com.bonus630.thefrog.Debuggers;
using br.com.bonus630.thefrog.Items;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace br.com.bonus630.thefrog.Player
{
    [DefaultExecutionOrder(-1), SelectionBase]
    public class Player : MonoBehaviour, IPlayer, IForcible
    {
        [SerializeField] private GameObject interactIcon;
        [SerializeField] private GameObject bar;
        [SerializeField] private GameObject footer;
        [SerializeField] private GameObject projectile;
        //[SerializeField] private GameObject fireball;
        //[SerializeField] private GameObject lightningBolt;
        [SerializeField] private Transform projectilesSpawPoint;
        [SerializeField] private Transform projectilesSpawPoint2;

        public int CurrentLife { get { return playerHealth.CurrentLife; } set { playerHealth.CurrentLife = value; } }
        [Header("Sounds")]
        [SerializeField] private AudioClip throwProjectileSFX;
        [SerializeField] private AudioClip Entrace;
        [SerializeField] private AudioClip PowerUp;
        [Header("Effects")]
        [SerializeField] private ParticleSystem GravityParticles;
        [SerializeField] private VisionController visionController;
        public BarManager barManager { get; set; }
        public PlayerManager playerManager { get; set; }

        public PlayerDialogue playerDialogue { get; private set; }
        public PlayerHealth playerHealth { get; private set; }
        public PlayerMovement playerMovement { get; private set; }
        public PlayerFallControl playerFallControl { get; private set; }
        public PlayerSpiritController playerSpiritController { get; private set; }
        public PlayerDirector playerDirector { get; private set; }

        [Header("Others")]
        private Rigidbody2D rb;
        [SerializeField] public Animator anim;
        public WallCheck WallCheck { get; private set; }
        private BoxCollider2D footerCollider;
        private AudioSource audioSource;
        /// <summary>
        /// -1 para normal, 1 para ponta cabe�a
        /// </summary>
        public float gravityDirection = 1;
        public float gravityScale = 4f;
        public bool knockUp { get; set; } = false;
        public bool IsJumpPressed { get; set; }
        [SerializeField] public Vector2 knockUpForce;
        public bool InGround { get; set; }
        private bool inputsOn = true;

        public float LookFor { get; set; } = 1;

        public GameObject FooterColliding { get { return footer; } protected set { footer = value; } }
        public bool MoveInputOn { get { return inputsOn; } set { inputsOn = value; } }

        //public float Speed { get { return playerMovement.Speed; } set { playerMovement.Speed = value; } }
        // public float JumpForce { get { return playerMovement.JumpForce; } set { playerMovement.JumpForce = value; } }

        public float RigibodyGravityScale { get { return rb.gravityScale; } set { rb.gravityScale = value; } }
        public float RigibodyLinearVelocityY { get { return rb.linearVelocityY; } set { rb.linearVelocityY = value; } }
        public float RigibodyLinearVelocityX { get { return rb.linearVelocityX; } set { rb.linearVelocityX = value; } }
        public Vector2 RigibodyLinearVelocity { get { return rb.linearVelocity; } set { rb.linearVelocity = value; } }

        public RigidbodyType2D RigibodyBodyType { get { return rb.bodyType; } set { rb.bodyType = value; } }

        public event System.Action<float> GravityChanged;
        private  Scene bornScene; 
        void Awake()
        {
            GetComponents();
            bornScene = SceneManager.GetActiveScene();
            //jumpTimeCharger = startJumpTime;
        }
        private void GetComponents()
        {
            rb = GetComponent<Rigidbody2D>();
            //anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            footerCollider = footer.GetComponent<BoxCollider2D>();
            WallCheck = GetComponent<WallCheck>();
            playerDialogue = GetComponent<PlayerDialogue>();
            playerHealth = GetComponent<PlayerHealth>();
            playerMovement = GetComponent<PlayerMovement>();
            playerDirector = GetComponent<PlayerDirector>();
            playerFallControl = GetComponent<PlayerFallControl>();
            playerSpiritController = GetComponent<PlayerSpiritController>();
            barManager = GetComponent<BarManager>();
            playerManager = GetComponent<PlayerManager>();
            playerManager.GameEventChange(() => { playerSpiritController.GameEventsChanged(); });


            //Debug.Log("Player getcomponentes:" + playerManager.PlayerStates);
            ServiceLocator.Instance.Register<PlayerInput>(GetComponent<PlayerInput>());
            ServiceLocator.Instance.Register<IPlayer>(this);
            ServiceLocator.Instance.Register("Player", gameObject);
            
        }
        private void Start()
        {
            //#if !UNITY_EDITOR
            //            //Debug.Log(GameManager.Instance.ToString());
            //            //Debug.Log(GameManager.Instance.PlayerStates.ToString());
            //            //Debug.Log(GameManager.Instance.PlayerStates.PlayerPosition.ToString());
            transform.position = GameManager.Instance.PlayerStartPosition;
            if (transform.position == GameObject.Find(GameManager.Instance.StartPointBuilder).gameObject.transform.position)
            {
                audioSource.PlayOneShot(Entrace);
                //rb.AddForce(new Vector2(100, 480), ForceMode2D.Impulse);
                AddForce(new Vector2(100, 480), ForceMode2D.Impulse, 4, true);
                return;
            }
            //Debug.Log("[Player] position " + transform.position);
            //if (SceneManager.GetActiveScene().name != "Main")
            //    return;
           // StartCoroutine(WaitSceneLoad());


            // if (transform.position == GameObject.Find(GameManager.Instance.StartPointBuilder).gameObject.transform.position)
            //Debug.Log("[Player] position object" + GameObject.Find(GameManager.Instance.StartPointBuilder).gameObject.transform.position);
            // Debug.Log("[Player] position player" + transform.position);
            // Debug.Log("[Player] gamemanager" + GameManager.Instance);
            //#else
            //            if (SceneManager.GetActiveScene().name.Equals(GameManager.Instance.InternAreas))
            //                transform.position = GameManager.Instance.PlayerStartPosition;
            //var i = FindAnyObjectByType<CamerasController>();
            //i.ActiveCam(2);
            ////           // playerMovement.FallsControl();
           // #endif

        }

        private IEnumerator WaitSceneLoad()
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            var sceneManager = ServiceLocator.Instance.Get<SceneLoadManager>();
            int blockIndex = -1; //sceneManager.GetBlockIndexByPosition(transform.position);
            yield return new WaitUntil(() =>
            {
                blockIndex = sceneManager.GetBlockIndexByPosition(transform.position);
                return blockIndex != -1;
            });
            Debug.Log("[Player] block index:" + blockIndex);
            sceneManager.LoadSceneAsync(blockIndex);
            bool wait = true;
            System.Action<List<string>> handler = null;
            handler = (list) =>
            {
                for (int i = 0; i < list.Count; i++)
                    Debug.Log("[Player] allscenes loaded scene:" + list[i]);
                wait = false;
            };
            sceneManager.AllScenesLoadedEvent += handler;
            yield return new WaitUntil(() => wait == false);
            sceneManager.AllScenesLoadedEvent -= handler;
            rb.bodyType = RigidbodyType2D.Dynamic;
            Debug.Log("[Player] scene loaded, player dynamic");
        }

        //public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse, float time = 1f)
        //{
        //   AddForce(force, mode, time);
        //}
        //public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse, float time = 1f,bool removeInput = true)
        //{
        //    if(removeInput)
        //        StartCoroutine(RemoveInputs(time));
        //    playerMovement.UseYvelocityLimit = false;
        //    rb.AddForce(force, mode);
        //    Invoke(nameof(ReenableYVelocityLimit), 0.6f);
        //}
        // private void ReenableYVelocityLimit() => playerMovement.UseYvelocityLimit = false;
        public void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse, float time = 1f, bool removeInput = true)
        {
            rb.AddForce(force, ForceMode2D.Impulse);
            StartCoroutine(IgnoreDampingForDuration(time, removeInput));
        }

        IEnumerator IgnoreDampingForDuration(float duration, bool removeInput)
        {
            playerMovement.IgnoreDamping = true;
            if (removeInput)
                inputsOn = false;
            float timer = 0f;
            playerMovement.UseYvelocityLimit = false;
            //    rb.AddForce(force, mode);
            Invoke(nameof(ReenableYVelocityLimit), 0.6f);
            while (timer < duration && Mathf.Abs(rb.linearVelocity.x) >= 0.1f)
            {
                // se a velocidade j� caiu, pode desligar antes
                //if (Mathf.Abs(rb.linearVelocity.x) < 0.1f)
                //    break;

                timer += Time.fixedDeltaTime; // usa ciclo da f�sica
                yield return new WaitForFixedUpdate();
            }
            inputsOn = true;
            if (duration > 0 && !removeInput)
                playerMovement.IgnoreDamping = false;
            // DebugUtils.Log("InputsOn: " + inputsOn);
        }
        private void ReenableYVelocityLimit() => playerMovement.UseYvelocityLimit = true;


        private void Update()
        {

            //RaycastHit2D hit2D = Physics2D.BoxCast(Vector2.zero, new Vector2(0.140f, 0.01f), 0, Vector2.down);
            //Gizmos.DrawWireCube(Vector3.zero, new Vector3(0.140f, 0.01f));

#if UNITY_EDITOR
            //if (Input.GetKeyUp(KeyCode.W))
            //{
            //    GameManager.Instance.GetPlayerScript.UpdatePlayer();
            //    //CreateBar(Color.green,0.4f);
            //    //foreach (var c in g.GetComponents(typeof(IBarUI)))
            //    //{
            //    //    Debug.Log(c.GetType().IsAssignableFrom(typeof(IBarUI)));
            //    //    (c as IBarUI).GoToValue(50);
            //    //}

            //    //playerMovement.FallsControl();
            //    //GameManager.Instance.TesteThumb();
            //    //ScreenEffects s  = GameObject.FindAnyObjectByType<ScreenEffects>();
            //    //StartCoroutine(DestroyEffects(s));
            //    //Debug.Log(s.camerasController);
            //    //s.ScreenAndGamepadShake();
            //    //s.FadeOut();
            //    //Debug.Log(s);
            //    //MusicSource m = FindAnyObjectByType<MusicSource>();
            //    //m.CrossFade(BackgroundMusic.AppleTree);
            //    // GameObject.Find("Virtual Camera").GetComponent<Animator>().SetTrigger("Shake");
            //    //GameManager.Instance.UpdatePlayer();
            //    //GameObject.FindAnyObjectByType<CamerasController>().ShakeCameraEffect();
            //}
            //if (Input.GetKeyUp(KeyCode.T))
            //{
            //    GameManager.Instance.StartTimer(10, () => { Debug.Log("Time Over Event"); });
            //    //GameManager.Instance.TimeOverEvent += () => { Debug.Log("Time Over Event"); };
            //}
            //if (Input.GetKeyUp(KeyCode.Alpha9))
            //    GameManager.Instance.UpdateMaxHearts(1);
            //if (Input.GetKeyUp(KeyCode.Alpha8))
            //    GameManager.Instance.UpdateHeart(-1);
            //if (Input.GetKeyUp(KeyCode.Alpha1))
            //    GameManager.Instance.SaveStates(1);
            //if (Input.GetKeyUp(KeyCode.Alpha2))
            //{
            //    GameManager.Instance.ChangeGameToState(GameManager.Instance.LoadStates(1));
            //}
            //if (Input.GetKeyUp(KeyCode.Alpha7))
            //{
            //    GameManager.Instance.PlayerStates.CollectablesID.Add("Apple_" + Random.Range(0, 10000));
            //    GameManager.Instance.PlayerStates.Collectables++;
            //    GameManager.Instance.UpdateScore();
            //}
            //if (Input.anyKeyDown)
            //{
            //    switch (true)
            //    {
            //        case bool _ when Input.GetKeyDown(KeyCode.Keypad1):
            //            Time.timeScale = 0.1f; break;
            //        case bool _ when Input.GetKeyDown(KeyCode.Keypad2):
            //            Time.timeScale = 0.2f; break;
            //        case bool _ when Input.GetKeyDown(KeyCode.Keypad3):
            //            Time.timeScale = 0.3f; break;
            //        case bool _ when Input.GetKeyDown(KeyCode.Keypad4):
            //            Time.timeScale = 0.4f; break;
            //        case bool _ when Input.GetKeyDown(KeyCode.Keypad5):
            //            Time.timeScale = 0.5f; break;
            //        case bool _ when Input.GetKeyDown(KeyCode.Keypad6):
            //            Time.timeScale = 1f; break;
            //        case bool _ when Input.GetKeyDown(KeyCode.Keypad7):
            //            Time.timeScale = 1.5f; break;
            //        case bool _ when Input.GetKeyDown(KeyCode.Keypad8):
            //            Time.timeScale = 2f; break;
            //        case bool _ when Input.GetKeyDown(KeyCode.Keypad9):
            //            Time.timeScale = 3f; break;
            //    }
            //    Debug.LogWarning("Time Scale: " + Time.timeScale);
            //}

#endif

        }
        int bars = 0;

        IEnumerator DestroyEffects(ScreenEffects screenEffects)
        {
            yield return new WaitForEndOfFrame();
            screenEffects.StartCameraShake(2, 2);
            screenEffects.GamepadShake(0.5f, 0.1f);
            yield return new WaitForSeconds(1f);
            screenEffects.StopCameraShake();
            screenEffects.GamepadShake(0f, 0f);
        }

        private float nextLaunch = 0f;
        public void LaunchSpirit()
        {
            if (playerSpiritController.CurrentProjectile == null)
                return;
            if (Time.time > nextLaunch)
            {
                //GameObject bullet = Instantiate(playerSpiritController.CurrentProjectile.Projectil, playerSpiritController.CurrentProjectile.SpawnPoint.transform.position, Quaternion.identity);
                GameObject bullet = Instantiate(playerSpiritController.CurrentProjectile.Projectil,
                    playerMovement.GetWallSliding ? playerSpiritController.CurrentProjectile.SpawnPoint2.transform.position : playerSpiritController.CurrentProjectile.SpawnPoint.transform.position,
                    Quaternion.identity);

                if (bullet != null && bullet.TryGetComponent<IProjectilies>(out IProjectilies projectilie))
                {
                    if (gravityDirection.Equals((float)PlayerGravityDirection.UP))
                        projectilie.ChangeDirectionY();
                    projectilie.Launch(new Vector2(LookFor.FlipIfNegative(playerMovement.GetWallSliding), 0));
                    IBarUI bar = barManager.CreateBar(playerSpiritController.CurrentProjectile.EffectColor, 0, transform, gravityDirection);
                    bar.MaxValue = 100;
                    bar.GoToValue(100, projectilie.ReloadTime());
                    bar.DestroyBar(projectilie.ReloadTime());
                    nextLaunch = Time.time + projectilie.ReloadTime();
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

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

            if (collision.gameObject.layer == 13)
            {
                try
                {
                    gameObject.transform.parent = null;
                    SceneManager.MoveGameObjectToScene(gameObject, bornScene);// evita que o player fique na cena da plataforma
                }
                catch (System.Exception e)
                {
                    Debug.LogError("[Player] Erro ao desparentear o player da plataforma: " + e.Message);
                }
            }
        }

        public void ActiveVision()
        {
            //  Debug.Log("[Player] HasVision:" + playerManager.PlayerStates.HasVision);
            if (playerManager.PlayerStates.HasVision)
                visionController.ActiveVision(barManager, gravityDirection);
        }

        public void ChangeGravity(float gravityDirection, float speed = 0.05f)
        {
            this.gravityDirection = gravityDirection;

            //LinearMaxY *= -1;
            playerManager.ActiveSkill(this.gravityDirection > 0);
            if (this.gravityDirection > 0)
            {
                // Debug.Log(gravityDirection);
                var m = GravityParticles.main;
                m.gravityModifierMultiplier = 0;
                GravityParticles.Play();
                StartCoroutine(ChangeGravityIenumerator(speed));
            }
            else
            {
                GravityParticles.Stop();
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
                rb.gravityScale *= -1;
                knockUpForce *= -1;
                GravityChanged?.Invoke(this.gravityDirection);
                barManager.ChangeBarDirection(this.gravityDirection);
                playerMovement.GravityChanged();
            }
        }
        private IEnumerator ChangeGravityIenumerator(float speed)
        {
            yield return new WaitForSeconds(speed);
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1, transform.localScale.z);
            var m = GravityParticles.main;
            m.gravityModifierMultiplier = -1;
            GravityChanged?.Invoke(this.gravityDirection);
            barManager.ChangeBarDirection(this.gravityDirection);
            playerMovement.GravityChanged();
            rb.gravityScale *= -1;

        }
        public void RemoveGravity(bool remove)
        {
            if (remove)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0;
            }
            else
                rb.gravityScale = this.gravityScale;
        }
        public void Alert()
        {
            interactIcon.SetActive(true);
            Invoke(nameof(Dealert), 2f);
        }
        private void Dealert()
        {
            interactIcon.SetActive(false);
        }
        public void Launch()
        {
            //bug
            //o nº foi alterado ao pegar uma shuryken
            if (playerManager.PlayerStates.Shurykens > 0)
            {
                GameObject projectileGO = playerMovement.GetWallSliding ? Instantiate(projectile, projectilesSpawPoint2.position, Quaternion.identity) : Instantiate(projectile, projectilesSpawPoint.position, Quaternion.identity);
                Shuryken projectileScript = projectileGO.GetComponent<Shuryken>();
                projectileScript.Launch(LookFor.FlipIfNegative(playerMovement.GetWallSliding), 10f);
                //animator.SetTrigger(launchHash);
                audioSource.PlayOneShot(throwProjectileSFX);
                ChangeNumberShurykens(-1);
            }
        }

        public IEnumerator RemoveInputs(float time = 0.2f)
        {
            inputsOn = false;
            yield return new WaitForSeconds(time);
            inputsOn = true;
        }
        public void KnockUpOnJump(Vector2 force)
        {
            knockUp = true;
            if (IsJumpPressed)
                force.y *= 1.5f;
            if (gravityDirection == 1)
                force.y *= -1;
            knockUpForce = force * 2;
            playerMovement.TimeInFastFall = 0;
            playerMovement.airDash = false;
            Debug.Log("[player] knockuponjump force:" + force);
        }
        public void KnockedUpOnHit(Vector2 force)
        {
            knockUp = true;
            knockUpForce = force;
            Debug.Log("[player] knockeduponhit force:" + force);
            ApplyKnockUp();

        }
        //public void KnockedUpOnHit()
        //{
        //    if (knockUp)
        //    {
        //        if (rb == null)
        //        {
        //            Debug.LogError("Rigidbody2D est� null no Build!");
        //            return;
        //        }
        //        //vou resetar o airDash aqui, mas n�o � o lugar certo para isso
        //        //playerMovement.airDash
        //        Debug.Log("knocked hit: " + knockUpForce);
        //        rb.linearVelocity = Vector2.zero;
        //        AddForce(knockUpForce, time: 0.2f, removeInput: false);
        //        //rb.AddForce(knockUpForce, ForceMode2D.Impulse);
        //        //pq estou estou resetando o timeinfastfall aqui?
        //        //aparentemente utilizo este metodo para aplicar o knock de pulo e hit

        //        knockUp = false;
        //    }
        //}
        public void ApplyKnockUp()
        {
            if (knockUp)
            {
                if (rb == null)
                {
                    knockUp = false;
                    throw new System.Exception("Rigidbody2D NULL");
                }
                rb.linearVelocity = Vector2.zero;
                AddForce(knockUpForce, time: 0.2f, removeInput: false);
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
        public void Hit(int damage)
        {
            playerHealth.Hit(damage);
        }
        public void GameOver()
        {
            playerHealth.GameOver();
        }
        public bool FooterTouching(Collider2D collision)
        {
            return footerCollider.IsTouching(collision);
        }
        public bool BodyTouching(Collider2D collision)
        {
            if(collision == null)
                return false;
            if (GetComponent<CapsuleCollider2D>() == null)
                return false;
            return GetComponent<CapsuleCollider2D>().IsTouching(collision);
        }

        public bool BodyTouching(LayerMask layer)
        {
            CapsuleCollider2D col = GetComponent<CapsuleCollider2D>();
            if (col == null)
                return false;
          return col.IsTouchingLayers(layer);
          
        }
        public void ChangeNumberShurykens(int shurykens)
        {
            playerManager.UpdateShurykens(shurykens);
        }
        public void ReadDialogue()
        {
            playerDialogue.ReadDialogue();
        }
        public void CancelDialogue()
        {
            playerDialogue.CancelDialogue();
        }
        public void FallsControl()
        {
            playerMovement.FallsControl();
        }
        public void AllInputsOn(bool inputOn = true, float delayTime = 0, bool autoSwitch = false, float switchTime = 0) => StartCoroutine(disablesAllInputs(inputOn, delayTime, autoSwitch, switchTime));
        private IEnumerator disablesAllInputs(bool inputOn, float delayTime, bool autoSwitch, float switchTime)
        {
            yield return new WaitForSeconds(delayTime);
            GetComponent<PlayerInputHandler>().enabled = inputOn;
            GetComponent<PlayerInput>().enabled = inputOn;
            //Debug.Log("Disable inputs :" + inputOn);
            if (autoSwitch)
            {
                yield return new WaitForSeconds(switchTime);
                GetComponent<PlayerInputHandler>().enabled = !inputOn;
                GetComponent<PlayerInput>().enabled = !inputOn;
            }
        }
        public void FreezePlayer()
        {
            AllInputsOn(true);
            playerMovement.FreezePlayerMove();
        }
        public void UpdatePlayer()
        {
            audioSource.PlayOneShot(PowerUp);
            playerManager.UpdatePlayer();
        }

        public void AddAction(PlayerDirectorData action)=> playerDirector.AddAction(action);
        public void AddAction(Action action,float time)=> playerDirector.AddAction(new PlayerDirectorData(action,time));

        public PlayerStates GetPlayerStates => playerManager.PlayerStates;
    }
    public enum PlayerGravityDirection : short
    {
        DOWN = -1,
        UP = 1
    }
}
