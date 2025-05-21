using br.com.bonus630.thefrog.Manager;
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

        protected override void Awake()
        {
            base.Awake();
            invencibleTimer = invencibleTime;
            hitTime = invencibleTime - 0.517f;//tempo da animação de hit
            CurrentLife = GameManager.Instance.PlayerStates.Hearts;
        }

        private void FixedUpdate()
        {
            if (invencible)
            {
                invencibleTimer -= Time.deltaTime;
                if (invencibleTimer < hitTime)
                {
                    flag = !flag;
                    GetComponent<SpriteRenderer>().enabled = flag;
                }
                if (invencibleTimer < 0)
                {
                    invencible = false;
                    GetComponent<SpriteRenderer>().enabled = true;
                }
            }
            else
            {
                invencibleTimer = invencibleTime;
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.layer == 10)
                Die();
            if (collision.gameObject.layer == 6)
            {
                if (!player.FooterTouching(collision.collider))
                {

                    Hit();
                    player.knockUp = true;
                    player.knockUpForce = collision.GetContact(0).normal * 40 * -1;
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
        }
        public void Die()
        {
            player.playerMovement.FreezePlayerMove();
            CurrentLife = 0;
            Hit();
        }
        public void GameOver()
        {
            player.InputsOn = false;
            rb.linearVelocity = Vector2.zero;
            rb.gravityScale = 0;
            rb.bodyType = RigidbodyType2D.Static;
            player.FooterColliding.GetComponent<BoxCollider2D>().isTrigger = true;

            GameManager.Instance.GameOver();
        }

    }
}
