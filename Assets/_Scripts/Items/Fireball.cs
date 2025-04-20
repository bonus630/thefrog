using System.Runtime.InteropServices.WindowsRuntime;
using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Activators;
using Unity.VisualScripting;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class Fireball : IProjectilies
    {
        [SerializeField] float speed;
        [SerializeField] float intensity = 1;
        [SerializeField] AudioClip launching;
        [SerializeField] AudioClip hitting;

        AudioSource audioSource;
        //Vector3 direction;
        Rigidbody2D rb;

        bool remove = false;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            //direction = transform.forward;
            audioSource = GetComponent<AudioSource>();
        }
        void Start()
        {

            

        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(transform.position.x);
        }


        public override void Launch(Vector2 direction)
        {
            if (rb != null)
            {
                audioSource.PlayOneShot(launching);
                rb.AddForce(direction * speed, ForceMode2D.Impulse);
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!remove)
            {
                remove = true;
                audioSource.PlayOneShot(hitting);
                GetComponent<Animator>().SetTrigger("Hit");
                Player player;
                if (collision.gameObject.TryGetComponent<Player>(out player))
                {
                    player.Hit();
                    return;
                }
                IEnemy enemy;
                if (collision.gameObject.TryGetComponent<IEnemy>(out enemy))
                {
                    enemy.Hit(intensity);
                    return;
                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("FireBall collision:"+collision.gameObject.name);
            if (!remove)
            {
              
                IEnemy enemy;
                if (collision.gameObject.TryGetComponent<IEnemy>(out enemy))
                {
                    enemy.Hit(intensity);
                    return;
                }
            }
        }
        public void Destroy()
        {
            // Debug.Log("FireBall Destroy");
            Destroy(gameObject);
        }

        public override Elements GetElement() => Elements.Fire;

        public override float ReloadTime() => 5f;
    }

}