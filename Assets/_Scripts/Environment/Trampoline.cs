using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class Trampoline : MonoBehaviour
    {
        [SerializeField] private float force;
        [SerializeField] private AudioClip boingSFX;
        private Animator anim;
        private AudioSource audioSource;
        private void Start()
        {
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }
        void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + transform.up * 2f);
            
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
                if (collision.transform.parent != null && collision.transform.parent.gameObject.layer == 13)
                    collision.transform.SetParent(null);
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = transform.up.normalized;
                float massFactor = rb.mass;
                float gravityFactor = rb.gravityScale;
                float adjustedForce = force * massFactor * gravityFactor;

                rb.AddForce(direction * adjustedForce, ForceMode2D.Impulse);
                rb.AddForce(force * direction, ForceMode2D.Impulse);
                anim.SetTrigger("active");
                audioSource.PlayOneShot(boingSFX);
            }
            //Debug.Log(rb);
        }
    }
}
