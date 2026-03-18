using System.Collections;
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
        public bool InHit { get; private set; }

        private void Start()
        {
            CurrentLife = player.playerManager.PlayerStates.Hearts;
#if UNITY_EDITOR
            
            CurrentLife = 18;
            player.playerManager.UpdateHeart(18);
#endif
        }

        protected override void Awake()
        {
            base.Awake();
            invencibleTimer = invencibleTime;
            hitTime = invencibleTime - 0.517f;//tempo da animação de hit
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
                StartCoroutine(Die());
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.layer == 6)
            {
                if (!player.FooterTouching(collision.collider))
                {
                    Hit();
                    if (collision.collider.TryGetComponent<IEnemy>(out IEnemy enemy))
                    {
                        player.knockUp = true;
                        player.KnockedUpOnHit(collision.GetContact(0).normal.x * enemy.KnockUpHitForce);
                        //player.knockUpForce = Vector2.Scale(collision.GetContact(0).normal, enemy.KnockUpHitForce);
                    }
                }
            }
            if (collision.gameObject.layer == 10)
            {
                if (!invencible)
                    StartCoroutine(Die());
            }
        }
        public void Hit(int damage = 1)
        {
            if (invencible)
                return;
            Debug.Log("[PlayerHealth][Hit]");
            player.playerManager.UpdateHeart(-damage);
            invencible = true;
            anim.SetTrigger(HitID);
            audioSource.PlayOneShot(hitSFX);
            anim.SetInteger(LifeID, CurrentLife);
            this.InHit = true;
            Invoke(nameof(RestoreHit), hitTime);
            if (CurrentLife <= 0)
            {
                Invoke(nameof(GameOver), 0.517f);
            }
            try
            {
                ServiceLocator.Instance.Get<ScreenEffects>().FashVignettePlayerDamage();
            }
            catch { }
        }
        private void RestoreHit() => this.InHit = false;
        public IEnumerator Die()
        {
            player.playerMovement.FreezePlayerMove();
            int life = CurrentLife;
            CurrentLife = 0;
            Hit();
            while (life > 0)
            {
               // Debug.Log("[PlayerHealth][Die]");
                player.playerManager.UpdateHeart(-1);
               
                yield return null;
            }
            yield return null;
            
           // Debug.Log("[PlayerHearth] prepareFallDie:" + PrepareFallDie);
            //player.AllInputsOn(false);
        }
        public void GameOver()
        {
            player.AllInputsOn(false);
            player.RigibodyLinearVelocity = Vector2.zero;
            player.RigibodyGravityScale = 0;
            player.RigibodyBodyType = RigidbodyType2D.Kinematic;
            player.FooterColliding.GetComponent<BoxCollider2D>().isTrigger = true;
            player.playerManager.PlayerDie();

        }

    }
}
