using br.com.bonus630.thefrog.Environment;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class RockBoss : EnemyToad
    {
        [SerializeField] AudioClip[] walkAudio;
        [SerializeField] AudioClip rockFalling;
        [SerializeField] AudioSource source;
        [SerializeField] Stalagmite stalagmiteScript;
        [SerializeField] GameObject player;
        [SerializeField] bool followPlayer = false;
        [SerializeField] float followTime = 0f;

        protected override void Start()
        {
            base.Start();
            player = GameManager.Instance.GetPlayer;
        }
        protected override void Update()
        {
            if (followPlayer)
            {
                int playerDirection = (player.transform.position.x > transform.position.x) ? 1 : -1;
                if (playerDirection != xDirection)
                    ChangeDirection();
                followTime = 0f;
                this.speed = 100f;
            }
            else
            {
                this.speed = 80f;
                followTime += Time.deltaTime;
            }
            if (frontColliding)
                Debug.Log("FrontCollider:");
            if (frontColliding && followTime > 4f)
                followPlayer = true;
            else if (frontColliding && followPlayer)
                followPlayer = false;
            base.Update();

        }
        protected override void OnCollisionEnter2D(Collision2D collision)
        {

        }

        public override void Hit(float hit)
        {
            animator.SetTrigger(HitID);
            this.life = this.life - hit;
            if (life < 0.1f)
            {
                stalagmiteScript.Activate();
                source.PlayOneShot(rockFalling);
                FindAnyObjectByType<CamerasController>().ShakeCameraEffect();
                Destroy(gameObject, 0.2f);
            }
        }
        public void PlayStepAudio()
        {
            source.PlayOneShot(walkAudio[Random.Range(0, walkAudio.Length)]);
        }

    }
}
