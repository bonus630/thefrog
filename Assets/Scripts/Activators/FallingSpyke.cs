using System;

using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Assets.Scripts.Activators
{
    public class FallingSpyke :  IActivator
    {
        [SerializeField] Rigidbody2D rb;
        public override void Activate()
        {
            rb.gravityScale = 1;
        }

        public override void Deactive()
        {
            
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag("Ground"))
                Destroy(gameObject);
        }
    }
}
