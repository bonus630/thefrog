using System.Collections;
using br.com.bonus630.thefrog.Effects;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;
using UnityEngine.Playables;

namespace br.com.bonus630.thefrog.Enemies
{
    public sealed class Wizard : EnemyBase
    {
        enum State
        {
            Idle = 0,
            Flying = 1,
            Attacking = 2,
            Hitted,
            Shield = 10
        }

        Vector3 startPos;
        Vector3 imutyPos;
        Bounds ArenaBounds;
        readonly int FlyID = Animator.StringToHash("fly");
        readonly int AttackID = Animator.StringToHash("Attack");
        readonly int Attack2ID = Animator.StringToHash("Attack2");
        readonly int HittedID = Animator.StringToHash("Hitted");

        float timer = 0;
        bool flying = false;
        private System.Action[] action;
        float flyTimer = 0;
        float tolerance = 0.5f;
        float shieldTimer = 0f;
        float shieldTime = 0.1f;
        float healMagicTime = 0;
        System.Action AlignedAction = null;

        [SerializeField] GameObject plantBulletPrefab;
        [SerializeField] GameObject transformBulletPrefab;
        [SerializeField] GameObject player;
        [SerializeField] GameObject shield;
        [SerializeField] GameObject heal;
        [SerializeField] CollisionRelayEx hand;
        [SerializeField] CollisionRelayEx ProjectiliesSensor;
        [SerializeField] IActivator activeOnDie;
        [SerializeField] Transform magicSpawnerPoint;
        [SerializeField] LayerMask transformLayers;
        [SerializeField] State currentState = State.Idle;
        [SerializeField] PlayableDirector playableDirector;
        [SerializeField] BoxCollider2D Arena;
        protected override void Start()
        {
            startPos = transform.position;
            imutyPos = transform.position;
            ArenaBounds = Arena.bounds;
            player = ServiceLocator.Instance.Get("Player");
            ProjectiliesSensor.OnTriggerEnterAction += ProjectiliesSensor_OnTriggerEnterAction;
            hand.OnTriggerEnterAction += Hand_OnTriggerEnterAction;
            action = new System.Action[]
            {

                Fly,Fly,
                Attack,Attack,Attack2,
                GoHigh,GoHigh,GoHigh,
                Idle,
                Heal
            };
            base.Start();
        }

        private void Hand_OnTriggerEnterAction(ColliderData obj)
        {
            if (obj.ColliderOther.TryGetComponent<IPlayer>(out IPlayer player))
            {
                player.Hit(2);
                KnockUpHitForce = new Vector2(10, 10);
            }
        }

        private void ProjectiliesSensor_OnTriggerEnterAction(ColliderData obj)
        {
            Debug.Log("shield");
            if (obj.ColliderOther.TryGetComponent<IProjectilies>(out var proj))
            {
                Shielded();
            }
        }

        protected override void Update()
        {
            if (currentState == State.Idle)
            {
                healMagicTime += Time.deltaTime;
                timer += Time.deltaTime;
                if (timer > 1)
                {
                    timer = 0;
                    action[Random.Range(0, action.Length)]?.Invoke();
                }
            }
            if (currentState == State.Flying)
            {
                Vector2 pos = Vector2.MoveTowards(transform.position, startPos, speed * Time.deltaTime);
                hand.gameObject.SetActive(true);
                transform.position = pos + Vector2.up * (Mathf.Sin((flyTimer - Time.time) * 10) * Time.deltaTime);

                if (Mathf.Approximately(transform.position.x, startPos.x))
                {

                    FaceToPlayer();
                    Idle();
                    // spriteAfterImage?.Deactivate();
                }
            }
            else
                hand.gameObject.SetActive(false);
            if (currentState == State.Shield)
            {
                shieldTimer += Time.deltaTime;
                if (shieldTimer >= shieldTime)
                {
                    shieldTimer = 0f;
                    DisableShield();
                }
            }

            if (SameHeight())
            {
                AlignedAction?.Invoke();
                AlignedAction = null;
            }
        }
        private bool SameHeight() => Mathf.Abs(player.transform.position.y - transform.position.y) <= tolerance;

        private void Attack()
        {
            currentState = State.Flying;
            flying = true;

            float x = transform.position.x;
            var bounds = CameraUtils.GetBounds2D();
            if (x < bounds.bottomLeft.x)
                x = bounds.bottomLeft.x;
            if (x < ArenaBounds.min.x)
                x = player.transform.position.x - 4;
            if (x > bounds.bottomRight.x)
                x = bounds.bottomRight.x;
            if (x > ArenaBounds.max.x)
                x = player.transform.position.x + 4;


            startPos = new Vector3(x, player.transform.position.y, transform.position.z);
            AlignedAction = () =>
            {
                currentState = State.Attacking;
                flying = false;
                animator.SetTrigger(AttackID);
            };
        }
        private void Attack2()
        {
            if (SameHeight())
                return;
            currentState = State.Attacking;
            flying = false;
            animator.SetTrigger(Attack2ID);

        }
        private void Heal()
        {
            if (healMagicTime > 40f && this.life <= 2)
            {
                Idle();
                this.life = 10;
                healMagicTime = 0;
                Instantiate(heal, transform.position, Quaternion.identity);
            }
        }
        public void LaunchAttack()
        {
            var go = Instantiate(plantBulletPrefab, magicSpawnerPoint.position, Quaternion.identity);
            go.GetComponent<PlantBullet>().Direction = xDirection;

        }
        public void LaunchAttack2()
        {
            var playerPos = player.transform.position;
            var normal = (playerPos - transform.position).normalized;
            RaycastHit2D cast = Physics2D.Raycast(transform.position, normal, 20, transformLayers);
            Debug.DrawRay(transform.position, normal, Color.red, 2f);
            if (cast)
            {

                var go = Instantiate(transformBulletPrefab, magicSpawnerPoint.position, Quaternion.identity);
                go.GetComponent<TransformMagicBullet>().finalPos = cast.point;
            }
            // go.GetComponent<PlantBullet>().Direction = xDirection;
        }
        private void Idle()
        {
            if (AlignedAction != null)
                return;
            flying = false;
            animator.SetBool(FlyID, false);
            currentState = State.Idle;
        }
        private void GoHigh()
        {
            if (AlignedAction != null)
                return;
            PrepareFly();
            float y = transform.position.y;
            if (y < 50)
                y += 10;
            startPos = new Vector3(transform.position.x, y, transform.position.z);
        }
        private void FaceToPlayer()
        {
            Vector3 playerPos = ServiceLocator.Instance.Get("Player").transform.position;
            float dist = (transform.position - playerPos).x;
            if (dist < 0 && xDirection == -1)
                ChangeDirection();
            if (dist > 0 && xDirection == 1)
                ChangeDirection();
            Idle();
            Debug.Log("[Wizard] FaceToPlayer distance:" + dist + " localScale" + transform.localScale);

        }
        SpriteAfterImageEffect spriteAfterImage;
        ushort effectID;
        private void Fly()
        {
            PrepareFly();
            startPos = new Vector3(transform.position.x + 10 * xDirection, transform.position.y, transform.position.z);
        }
        private void PrepareFly()
        {
            currentState = State.Flying;
            flying = true;
            animator.SetBool(FlyID, true);
            flyTimer = Time.time;
        }
        public void ActiveFlyEffect()
        {
            spriteAfterImage ??= EffectManager.instance.GetEffect<SpriteAfterImageEffect>(effectID) as SpriteAfterImageEffect;
            spriteAfterImage ??= SpriteAfterImageEffect.Create(GetComponent<SpriteRenderer>())
                                                        .WithLimit(4)
                                                        .WithLifeTime(3f)
                                                        .WithFadeSpeed(1f);
            spriteAfterImage.Activate();
            effectID = EffectManager.instance.AddEffect(spriteAfterImage);
        }
        private void OnDisable()
        {
            spriteAfterImage?.Deactivate();
            ProjectiliesSensor.OnTriggerEnterAction -= ProjectiliesSensor_OnTriggerEnterAction;
        }
        private void ChangeDirection()
        {
            xDirection *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        public override void Hit(float hit)
        {
            if (currentState == State.Shield || hitted)
                return;
            currentState = State.Hitted;
            hitted = true;
            Invoke(nameof(DisableCollider), 0.01f);
            animator.SetTrigger(HitID);
            this.life = this.life - hit;
            if (life <= 0)
                Die();

            //startPos = new Vector3(transform.position.x , transform.position.y - 10, transform.position.z);
            //currentState = State.Flying;

        }
        public void DisableCollider()
        {
            coll.enabled = false;
        }
        private bool hitted = false;
        public void Hitted()
        {

            animator.SetBool(HittedID, hitted);
            StartCoroutine(HittedPosition());
        }
        private IEnumerator HittedPosition()
        {
            float time = 0;
            Color t = new Color(1, 1, 1, 0);
            while (time < 0.6f)
            {
                GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, t, time);
                time += Time.deltaTime;
                yield return new WaitForSeconds(0.05f);
            }
            GetComponent<SpriteRenderer>().color = t;
            yield return null;
            transform.position = imutyPos;
            yield return null;
            hitted = false;
            animator.SetBool(HittedID, hitted);
            yield return null;
            GetComponent<SpriteRenderer>().color = Color.white;
            coll.enabled = true;
            currentState = State.Idle;
        }
        private void Die()
        {
            ServiceLocator.Instance.Get<MusicSource>().Sleep();
            ServiceLocator.Instance.Get<IPlayer>().AllInputsOn(false);
            playableDirector.stopped += (a) =>
            {
                GameManager.Instance.EventCompleted(GameEventName.DefeatWizard, true);
                ServiceLocator.Instance.Get<IPlayer>().UpdatePlayer();
                activeOnDie.Activate();
                ServiceLocator.Instance.Get<IPlayer>().AllInputsOn(true,1f);
            };
            action = new System.Action[0];
            coll.enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            playableDirector.Play();
            //
        }

        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player) && player.FooterTouching(coll))
                {
                    // Debug.Log("collision base");
                    player.KnockUpOnJump(Repulse);
                    Hit(1);
                    return;
                }
                ChangeDirection();
            }
            if (hitted && collision.gameObject.layer == 8)
            {

            }
            if (collision.gameObject.layer == 12)
            {
                if (collision.gameObject.TryGetComponent<IElement>(out var el))
                {
                    GetComponent<HitFlash>().SetColorHit(el.ElementColor);
                }
                Hit(0.5f);
            }
        }

        public void Shielded()
        {
            if (currentState != State.Idle)
                return;
            currentState = State.Shield;
            shield.SetActive(true);
        }
        public void DisableShield()
        {
            currentState = State.Idle;
            shield.SetActive(false);
        }
        public void OnAttackFinished() => Idle();
    }
}
