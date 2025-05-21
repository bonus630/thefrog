using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class Boxes : IActivator
    {
   
        [SerializeField] SpriteRenderer spriteRenderer;
        //[SerializeField] GameObject[] pierces;
        [SerializeField] AudioSource audioSource;
        [SerializeField] GameObject Item;
        public override void Activate()
        {
            
        }

        public override void Deactive()
        {
            Break();
        }
        private void Break()
        {
            spriteRenderer.enabled = false;
            audioSource.Play();
            if (Item != null)
            {
                int i = Random.Range(2, 6);
               for (;i > 0; i--)
                    Instantiate(Item, new Vector2(transform.position.x, transform.position.y + 0.4f), Item.transform.rotation);
            }
            for (int i = 0; i < transform.childCount; i++)
            {
               Rigidbody2D rb = transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>();
               
               rb.AddForce(new Vector2(Random.Range(-50, 50), Random.Range(60, 160)));
                rb.gravityScale = 1f;
            }
            Destroy(gameObject,1f);
        }
      
    }
}
