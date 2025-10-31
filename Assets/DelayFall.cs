using System.Collections;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;


namespace br.com.bonus630.thefrog
{
    [RequireComponent(typeof(Rigidbody2D),typeof(AudioSource),typeof(Collider2D))]
    
    public class DelayFall : MonoBehaviour
    {
        [SerializeField] float fallTime = 0.5f;
        [SerializeField] ParticleSystem fallEffect;
        [SerializeField] AudioClip fallSound;
        [SerializeField] AudioClip crashSound;
        [SerializeField] LayerMask collisionLayers;
        IEnumerator Start()
        {
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;
            fallEffect.Play();
            yield return new WaitForSeconds(fallTime);
            fallEffect.Stop();
            GetComponent<Collider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;
            if(fallSound!=null)
                GetComponent<AudioSource>().PlayOneShot(fallSound);
            GetComponent<Rigidbody2D>().gravityScale = 1f;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayerMask(collisionLayers))
            {
                Debug.Log("[DelayFall] collision: " + collision.gameObject.name);   
                float removeTime = 0;
                if (crashSound != null)
                {
                    GetComponent<AudioSource>().PlayOneShot(crashSound);
                    removeTime = crashSound.length;
                }
                Destroy(gameObject, removeTime);
            }
        }


    }
}
