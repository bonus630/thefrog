using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyGhost2 : EnemyGhostTrigger
    {
        private float invencibleTime = 2f;
        [SerializeField] private bool invencible = false;


        private Animator anim;


        protected override void Awake()
        {
            base.Awake();
            followTime = 10f;
            anim = GetComponent<Animator>();
        }

        protected override void Update()
        {
            FollowPlayer();
            invencibleTime -= Time.deltaTime;
            if (followTime < 0)
                invencibleTime = 2f;
            if (invencibleTime < 0)
            {
                invencible = false;
                // invencibleTime = 2f;
            }
            else
            {
                invencible = true;
                followTime = 10f;
            }

            anim.SetBool("Invencible", invencible);

        }
        private new void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player) && player.FooterTouching(coll) && !invencible)
                {
                    player.KnockUp(repulse);
                    Hit(1);
                    return;
                }
                else
                    player.Hit();

            }
        }
    }
}
