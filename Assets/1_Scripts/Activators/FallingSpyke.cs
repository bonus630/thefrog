using System;

using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class FallingSpyke :  IActivator
    {
        [SerializeField] Rigidbody2D rb;
        AudioSource audioSource;


        public void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public override void Activate()
        {
            audioSource.Play();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 1;
        }

        public override void Deactive()
        {
            
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Ground")||
                collision.gameObject.CompareTag("Platform"))
                Destroy(gameObject);
        }
    }
}
