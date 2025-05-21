using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyBridFly : EnemyBase, IEnemy
    {
        CircleCollider2D circleCollider;
        SpriteRenderer spriteRenderer;
        Vector3 left;
        Vector3 right;
        int cicle = 0;
        protected override void Awake()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            left = Camera.main.ViewportToWorldPoint(Vector3.zero);
            right = Camera.main.ViewportToWorldPoint(Vector3.right);
        }
        protected override void Start() { }
        protected override void Update()
        {
            transform.position += Vector3.right * speed;
            if (transform.position.x > right.x + 5 && speed > 0)
            {
                spriteRenderer.flipX = true;
                speed *= -1;

            }
            if (transform.position.x < left.x - 5 && speed < 0)
            {
                spriteRenderer.flipX = false;
                speed *= -1;
                cicle++;
                if (cicle > 5)
                {
                    transform.position = new Vector3(transform.position.x, GameManager.Instance.GetPlayer.transform.position.y, 0);
                    cicle = 0;
                }
            }
        }
   
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
               if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player) && player.FooterTouching(coll))
                {
                    player.KnockUp(repulse);
                    Destroy(gameObject, 0.05f);
                    return;
                }

            }
            if (collision.gameObject.layer == 12)
            {
                life -= 0.5f;
                if(life < 0.1f)
                    Destroy(gameObject);    
            }
        }
    }
}
