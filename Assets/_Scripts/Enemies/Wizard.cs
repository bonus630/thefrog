using br.com.bonus630.thefrog.Effects;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public sealed class Wizard : EnemyBase
    {
        Vector3 startPos;

        readonly int FlyID = Animator.StringToHash("fly");
        readonly int AttackID = Animator.StringToHash("Attack");
        readonly int Attack2ID = Animator.StringToHash("Attack2");

        float timer = 0;
        bool flying = false;
        private System.Action[] action;
        float flyTimer = 0;


        [SerializeField] GameObject plantBulletPrefab;
        [SerializeField] Transform magicSpawnerPoint;


        protected override void Start()
        {
            startPos = transform.position;
            action = new System.Action[]
            {
                Fly,Attack,Attack2,Idle
            };
            base.Start();
        }
        protected override void Update()
        {
            timer += Time.deltaTime;
            if (timer > 1)
            {
                timer = 0;
                action[Random.Range(0, action.Length)]?.Invoke();
            }
            if (flying)
            {
                Vector2 pos = Vector2.MoveTowards(transform.position, startPos, speed * Time.deltaTime);

                transform.position = pos + Vector2.up * (Mathf.Sin((flyTimer - Time.time) * 10) * Time.deltaTime);

                if (Mathf.Approximately(transform.position.x, startPos.x))
                {
                    
                    FaceToPlayer();
                    Idle();
                    spriteAfterImage?.Deactivate();
                }
            }
        }
        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }
        private void Attack()
        {
            flying = false;
            animator.SetTrigger(AttackID);
        }
        private void Attack2()
        {
            flying = false;
            animator.SetTrigger(Attack2ID);
           
        }
        public void LaunchAttack2()
        {
             var go = Instantiate(plantBulletPrefab, magicSpawnerPoint.position,Quaternion.identity);
            go.GetComponent<PlantBullet>().Direction = xDirection;
        }
        private void Idle()
        {
            flying = false;
            animator.SetBool(FlyID, false);
        }
        private void FaceToPlayer()
        {
            Vector3 playerPos = ServiceLocator.Instance.Get("Player").transform.position;
            float dist = (transform.position - playerPos).x;
            if (dist < 0 && xDirection == -1)
                ChangeDirection();
            if (dist > 0 && xDirection == 1)
                ChangeDirection();
            Debug.Log("[Wizard] FaceToPlayer distance:" + dist + " localScale" + transform.localScale);

        }
        SpriteAfterImageEffect spriteAfterImage;
        ushort effectID;
        private void Fly()
        {
            flying = true;
            animator.SetBool(FlyID, true);
            flyTimer = Time.time;
            startPos = new Vector3(transform.position.x + 10 * xDirection, transform.position.y, transform.position.z);
        }
        public void ActiveFlyEffect()
        {
            spriteAfterImage ??= EffectManager.instance.GetEffect<SpriteAfterImageEffect>(effectID) as SpriteAfterImageEffect;
            spriteAfterImage ??= new SpriteAfterImageEffect(GetComponent<SpriteRenderer>(), 4, lifeTime: 3f, fadeSpeed: 1f);
            spriteAfterImage.Activate();
            effectID = EffectManager.instance.AddEffect(spriteAfterImage);
        }
        private void ChangeDirection()
        {
            xDirection *= -1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        public override void Hit(float hit)
        {
            base.Hit(hit);

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
        }
    }
}
