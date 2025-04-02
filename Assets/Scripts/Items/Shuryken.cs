using br.com.bonus630.thefrog.Caracters;
using Unity.VisualScripting;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class Shuryken : CollectShuryken
    {
        Rigidbody2D rb;
        [SerializeField] Collider2D coll;
        // [SerializeField] GameObject shuryken;
        //  bool canClone = true;

        private void Update()
        {
            //Debug.Log("Shuryken: " + rb.linearVelocityX);

            //if (Mathf.Approximately(rigidbody.linearVelocityX, 0) && Mathf.Approximately(rigidbody.linearVelocityY, 0) && canClone)
            //{
            //    canClone = false;
            //    var o = Instantiate(shuryken, transform.position, shuryken.transform.rotation);
            //   // o.transform.parent = null;
            //    DestroyImmediate(gameObject);

            //}
            //if(Mathf.Abs(rb.linearVelocityX) < 0.1f)
            //{
            //    rb.gravityScale = 1;

            //}
        }
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }
        public void Launch(float direction, float force)
        {
            rb.AddForce(new Vector2(direction * force, 0));
        }
        protected new void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Collider Shyruken: " + collision.gameObject.name);
            Player player;
            if (collision.gameObject.TryGetComponent<Player>(out player))
            {
                player.ChangeNumberShurykens(Shurykens);

                Destroy(gameObject);
            }
            //Collider2D coll = gameObject.GetComponent<CapsuleCollider2D>();
            // coll.isTrigger = false;
            //  rigidbody.gravityScale = 1;

        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Collision Shyruken 2: " + collision.gameObject.name);
            if (collision.gameObject.CompareTag("Player"))
                return;
            if (collision.gameObject.layer == 8)
            {
                Vector2 normal = collision.contacts[0].normal;
                coll.isTrigger = true;
                return;
            }


            Debug.Log(collision.contacts[0].normal);
            Destroy(gameObject);

        }


    }

}
