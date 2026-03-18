using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace br.com.bonus630.thefrog.Items
{
    public class Fireball : IProjectilies, IElement
    {
        [SerializeField] float speed;
        [SerializeField] float intensity = 1;
        [SerializeField] float lifeTime = 4f;
        [SerializeField] bool removeByTime = false;
        [SerializeField] AudioClip launching;
        [SerializeField] AudioClip hitting;
        [SerializeField] Light2D light2D;
        [SerializeField] GameObject FireSprite;
        [field:SerializeField]public bool isActived { get; set; }

        AudioSource audioSource;
        //Vector3 direction;
        Rigidbody2D rb;
        bool remove = false;
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            //direction = transform.forward;
             TryGetComponent<AudioSource>(out audioSource);
        }
        void Start()
        {

            

        }

        // Update is called once per frame
        void Update()
        {
            if (lifeTime < 0 && removeByTime)
                Destroy(gameObject);
            lifeTime -= Time.deltaTime;
        }


        public override void Launch(Vector2 direction)
        {
            if (rb != null)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                if(direction.x < 0)
                {
                    Vector3 scale = transform.localScale;
                    scale.x = -Mathf.Abs(scale.x);
                    transform.localScale = scale;
                    angle += 180;
                }
                rb.rotation = angle;

                audioSource?.PlayOneShot(launching);
                
                rb.AddForce(direction.normalized * speed, ForceMode2D.Impulse);
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
           // Debug.Log("FireBall collision:" + collision.gameObject.name);
            
            if (!remove)
            {
                remove = true;
                audioSource?.PlayOneShot(hitting);
                GetComponent<Animator>().SetTrigger("Hit");
                if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player))
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
                if(collision.gameObject.TryGetComponent<IElement>(out IElement element))
                {
                    if(element.CanActiveBy().Equals(GetElement))
                    {
                        element.ActiveBy(GetElement);
                    }
                    if(element.CanDeactiveBy().Equals(GetElement))
                    {
                        element.DeactiveBy(GetElement);
                    }

                }
            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
           // Debug.Log("FireBall trigger :"+collision.gameObject.name);
            if (!remove)
            {
                IEnemy enemy;
                if (collision.gameObject.TryGetComponent<IEnemy>(out enemy))
                {
                    enemy.Hit(intensity);
                    return;
                }
                if (collision.gameObject.TryGetComponent<IElement>(out IElement element))
                {
                    if (element.CanActiveBy().Equals(GetElement))
                    {
                        element.ActiveBy(GetElement);
                    }
                    if (element.CanDeactiveBy().Equals(GetElement))
                    {
                        element.DeactiveBy(GetElement);
                    }

                }
            }
        }
        public void Destroy()
        {
            // Debug.Log("FireBall Destroy");
            Destroy(gameObject);
        }

        [field: SerializeField] public override Elements GetElement { get; set; } = Elements.Fire;
        [field: SerializeField] public Color ElementColor { get; set; } = Color.red;
        
        public Elements CanActiveBy() => Elements.Fire;
        public Elements CanDeactiveBy() => Elements.Water;
        public override float ReloadTime() => 5f;
       
        public void ActiveBy(Elements element)
        {
            ActiveDeactive(true);
        }

        public void DeactiveBy(Elements element)
        {
            ActiveDeactive(false);
        }
        private void ActiveDeactive(bool active)
        {
            if (light2D != null)
                light2D.gameObject.SetActive(active);
            if (FireSprite != null)
                FireSprite.gameObject.SetActive(active);
        }
        public override void ChangeDirectionY()
        {
        
        }
    }

}