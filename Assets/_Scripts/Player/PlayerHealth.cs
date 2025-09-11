using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Player
{
    [Tooltip("Controla a vida e danos do jogador")]
    public class PlayerHealth : PlayerBase
    {
        [field: SerializeField] public int CurrentLife { get; set; } = 2;
        [SerializeField] private AudioClip hitSFX;
        private bool invencible;
        private bool flag = true;
        private readonly float invencibleTime = 1.2f;
        private float invencibleTimer;
        private float hitTime;
        protected readonly int HitID = Animator.StringToHash("Hit");
        protected readonly int LifeID = Animator.StringToHash("Life");
        public bool PrepareFallDie { get; set; } = false;
        protected override void Awake()
        {
            base.Awake();
            invencibleTimer = invencibleTime;
            hitTime = invencibleTime - 0.517f;//tempo da animação de hit

            CurrentLife = GameManager.Instance.PlayerStates.Hearts;
#if UNITY_EDITOR
            CurrentLife = 20;
#endif
        }

        private void FixedUpdate()
        {
            if (invencible)
            {
                invencibleTimer -= Time.deltaTime;
                if (invencibleTimer < hitTime)
                {
                    flag = !flag;
                    anim.gameObject.SetActive(flag);
                    //GetComponent<SpriteRenderer>().enabled = flag;
                }
                if (invencibleTimer < 0)
                {
                    invencible = false;
                    anim.gameObject.SetActive(true);
                    //GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            else
            {
                invencibleTimer = invencibleTime;
            }
            if (PrepareFallDie && player.InGround)
                Die();
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
          //  Debug.Log("Player Hit Collision name: " + collision.gameObject.name);
            if (collision.gameObject.layer == 10)
            {
                // Debug.Log("PlayerHearth :" + collision.gameObject.name);
                Die();
            }
            if (collision.gameObject.layer == 6)
            {
                if (!player.FooterTouching(collision.collider))
                {
                    Hit();
                    if (collision.collider.TryGetComponent<IEnemy>(out IEnemy enemy))
                    {
                        player.knockUp = true;
                        player.knockUpForce = collision.GetContact(0).normal * enemy.KnockUpHitForce ;
                       // Debug.Log("Player Hit: "+ enemy.KnockUpHitForce);
                    }
                }
            }

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
            if (CurrentLife <= 0)
            {
                Invoke(nameof(GameOver), 0.517f);
            }
            try
            {
                FindAnyObjectByType<ScreenEffects>().FashVignettePlayerDamage();
            }
            catch { }
        }

        public void Die()
        {
            player.playerMovement.FreezePlayerMove();
            CurrentLife = 0;
            Hit();
        }
        public void GameOver()
        {
            player.MoveInputOn = false;
            player.RigibodyLinearVelocity = Vector2.zero;
            player.RigibodyGravityScale = 0;
            player.RigibodyBodyType = RigidbodyType2D.Static;
            player.FooterColliding.GetComponent<BoxCollider2D>().isTrigger = true;

            GameManager.Instance.GameOver();
        }

    }
}
