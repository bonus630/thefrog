using System.Collections;
using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class EnemyBridFly : MonoBehaviour, IEnemy
    {
        CircleCollider2D circleCollider;
        SpriteRenderer spriteRenderer;
        Vector3 left;
        Vector3 right;
        [SerializeField] float speed;
        [SerializeField] protected Vector2 repulse = Vector2.up * 200;
        int cicle = 0;
        private void Awake()
        {
            circleCollider = GetComponent<CircleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            left = Camera.main.ViewportToWorldPoint(Vector3.zero);
            right = Camera.main.ViewportToWorldPoint(Vector3.right);
        }
        private void Update()
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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Player player;
            if (collision.gameObject.TryGetComponent<Player>(out player) && player.FooterTouching(circleCollider))
            {
                player.KnockUp(repulse);
                circleCollider.enabled = false;
                Destroy(gameObject);
            }
        }

        public void Hit(float amount)
        {
            Destroy(gameObject);
        }
    }
}
