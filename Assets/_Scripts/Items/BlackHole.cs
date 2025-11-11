using br.com.bonus630.thefrog.Effects;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class BlackHolePortal : MonoBehaviour
    {
        public Transform center;
        public float gravityForce = 20f;
        public float maxSpeed = 5f;

        BlackHoleEffect ba;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Rigidbody2D playerRb = other.attachedRigidbody;
                if (playerRb != null)
                {
                    ba = new BlackHoleEffect(playerRb, center, gravityForce: 20f, maxSpeed: 5f);
                    EffectManager.instance.AddEffect(ba);
                    
                }
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                OnDisable();
            }
        }

        private void OnDisable()
        {
            if (ba != null)
            {
               // Debug.Log("effect blackhole");
                ba.Deactivate();
                ba = null;
            }
        }
       
    }

}
