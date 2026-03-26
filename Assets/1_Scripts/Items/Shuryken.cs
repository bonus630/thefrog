using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class Shuryken : CollectShuryken
    {
        Rigidbody2D rb;
        [SerializeField] Collider2D coll;
        [SerializeField] AudioSource hitting;
        [SerializeField] AudioClip hittingSound;
        [SerializeField] AudioClip collectedSound;

        
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
        private void FixedUpdate()
        {
            if (rb.linearVelocityX < 1f)
            {

                rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
                rb.linearVelocityY = 0.01f;
            }
            //if (rb.linearVelocity.magnitude < 0.1f)
            //    rb.linearVelocityY = -0.5f;
        }
        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
            //Debug.Log("shury layer: "+gameObject.layer);
        }
        public void Launch(float direction, float force)
        {
            rb.linearVelocityX = direction * force;
            //rb.AddForce(new Vector2(direction * force, 0));
        }
        protected new void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("Collider Shyruken: " + collision.gameObject.name);
            if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player))
            {
                player.ChangeNumberShurykens(Shurykens);
                ServiceLocator.Instance.Get<AudioEffects>().Play(collectedSound);
                Destroy(gameObject);
            }
            //Collider2D coll = gameObject.GetComponent<CapsuleCollider2D>();
            // coll.isTrigger = false;
            //  rigidbody.gravityScale = 1;

        }
        //meu codigo
        bool hitWall = false;
        //private void OnCollisionEnter2D(Collision2D collision)
        //{
        //   // Debug.Log("Collision Shyruken 2: " + collision.gameObject.name);
        //    if (collision.gameObject.CompareTag("Player"))
        //        return;
        //    if (collision.gameObject.layer == 8)
        //    {
        //        if (hitWall)
        //        {
        //            rb.gravityScale = 0;
        //            coll.isTrigger = true;
        //            rb.linearVelocity = Vector2.zero;
        //            rb.freezeRotation = true;
        //        }
        //        else
        //        {
        //            Vector2 normal = collision.GetContact(0).normal;
        //            StartCoroutine(ChangeHitWall());
        //            hitting.PlayOneShot(hittingSound);
        //            rb.freezeRotation = false;
        //            rb.constraints = RigidbodyConstraints2D.None;
        //            rb.gravityScale = 1;
        //            rb.AddForce(new Vector2(normal.x * Random.Range(2,5), Random.Range(2, 5)), ForceMode2D.Impulse);
        //            rb.AddTorque(Random.Range(2, 5));
        //        }
        //        return;
        //    }


        //    //Debug.Log(collision.contacts[0].normal);
        //    Destroy(gameObject);

        //}

        //IA
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
                return;

            if (collision.gameObject.layer == 8)
            {
               // Vector2 normal = collision.GetContact(0).normal;
                Vector2 normal = GetNormal(collision);

                if (hitWall)
                {
                    if (normal.y > 0.5f) // só para em chão
                    {
                        rb.gravityScale = 0;
                        coll.isTrigger = true;
                        rb.linearVelocity = Vector2.zero;
                        rb.freezeRotation = true;
                    }
                    else
                    {
                        // ainda não é chão → continua ricochete
                        
                        hitting.PlayOneShot(hittingSound);
                        Bounce(normal);
                    }
                }
                else
                {
                    StartCoroutine(ChangeHitWall());
                    hitting.PlayOneShot(hittingSound);
                    Bounce(normal);
                }

                return;
            }

            Destroy(gameObject);
        }
        int bounceCount = 0;
        void Bounce(Vector2 normal)
        {
            float factor = Mathf.Pow(0.5f, bounceCount);
            Debug.Log("factor:" + factor);
     

            rb.freezeRotation = false;
            rb.constraints = RigidbodyConstraints2D.None;
            rb.gravityScale = 1;

            Vector2 force = normal * Random.Range(2f * factor, 5f * factor);
            force += Vector2.up * Random.Range(2f * factor, 5f * factor);

            rb.AddForce(force, ForceMode2D.Impulse);
            rb.AddTorque(Random.Range(2f, 5f));
            bounceCount++;
        }
        private Vector2 GetNormal(Collision2D collision)
        {
            Vector2 normal = Vector2.zero;

            for (int i = 0; i < collision.contactCount; i++)
            {
                normal += collision.GetContact(i).normal;
            }

            normal.Normalize();
            return normal;
        }
        private IEnumerator ChangeHitWall()
        {
           /// yield return new WaitForSeconds(0.1f);
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            hitWall = true;
        }

    }

}
