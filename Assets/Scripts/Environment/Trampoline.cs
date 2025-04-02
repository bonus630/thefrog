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
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Rigidbody2D rb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(new Vector2(0f, force), ForceMode2D.Impulse);
                anim.SetTrigger("active");
                audioSource.PlayOneShot(boingSFX);
            }
            Debug.Log(rb);
        }
    }
}
