using br.com.bonus630.Enemies;
using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    //Script altamenta acoplado, para a batalha
    public class Stalagmite : MonoBehaviour
    {
        [SerializeField] float shakeTime = 0.05f;
        //  [SerializeField] Collider2D stupCollider;
        [SerializeField] GameObject stub;
        [SerializeField] GameObject _base;
        [SerializeField] ParticleSystem dust;
        [field: SerializeField] bool autoDestroy { get; set; } = true;
        [field: SerializeField] bool actived { get; set; } = true;

        float lifeTimer = 0;
        float timer = 0;
        float shakeIn = 0.01f;
        float flickTime = 0;
        bool prepareToDestroy = false;
        bool flag = false;
        bool falling = false;
        Rigidbody2D rb;
        Transform tf;



        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            tf = rb.GetComponent<Transform>();
            stub.GetComponent<CollisionRelay>().OnTriggerEnterAction += CheckTriggerEnter;
            if (actived)
                Active();
            // rb.gravityScale = 10f;
        }
        public void Active()
        {
            dust.Play();
            actived = true;
        }
        private void Update()
        {
            if (actived)
            {
                if (lifeTimer >= 0 && lifeTimer < 2f)
                {
                    if (timer > shakeTime)
                    {
                        transform.position = rb.transform.position + new Vector3(shakeIn, 0, 0);
                        shakeIn *= -1;
                        timer = 0;
                    }
                    timer += Time.deltaTime;
                }
                if (lifeTimer >= 2f && !falling)
                {
                    falling = true;
                    rb.gravityScale = 10f;
                    dust.Stop();
                    GetComponent<AudioSource>().Play();
                }
                if (prepareToDestroy)
                {
                    if (timer > 5f)
                    {
                        if (flickTime >= 0.01f)
                        {
                            _base.GetComponent<SpriteRenderer>().enabled = flag;
                            flag = !flag;
                            flickTime = 0;
                        }
                        flickTime += Time.deltaTime;

                    }


                    timer += Time.deltaTime;
                }
                lifeTimer += Time.deltaTime;
            }
        }
        private void OnDestroy()
        {
            stub.GetComponent<CollisionRelay>().OnTriggerEnterAction -= CheckTriggerEnter;
        }
        private void CheckTriggerEnter(Collider2D other)
        {

            if (other.CompareTag("Ground"))
            {
                
                if (other.TryGetComponent<RockBoss>(out RockBoss boss))
                {
                    boss.Hit(1);
                    Destroy(gameObject);
                }
            }
            if(other.gameObject.CompareTag("Destroyable"))
            {
                Destroy(other.gameObject);
            }
            if (other.TryGetComponent<Player>(out Player player))
            {
                player.Hit();
                Destroy(gameObject);
            }
            if (autoDestroy)
            {
                prepareToDestroy = true;
                timer = 0;
                Destroy(gameObject, 5.6f);
            }
        }

    }
}
