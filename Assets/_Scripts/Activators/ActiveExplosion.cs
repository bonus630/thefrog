using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Collider2D))]
    public class ActiveExplosion : IActivator
    {
        [SerializeField] GameObject explosion;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Active Explosoi:" + collision.tag);
            if (collision.CompareTag("Destroyable"))
            {
                StartCoroutine(Explosions(collision));
            }
        }

        IEnumerator Explosions(Collider2D collision)
        {
            System.Random rand = new System.Random();
            Bounds bounds = collision.bounds;
            Destroy(collision.gameObject);
            for (int i = 0; i < 10; i++)
            {
                GameObject explo = Instantiate(explosion, rand.Vector2FromRect(bounds), Quaternion.identity);
                float var = Random.Range(5, 10);
                explo.transform.localScale = new Vector3(var, var, 0);
                Destroy(explo, 2f);
                yield return new WaitForSeconds(0.1f);
            }
            //float time = 0.5f;
            //GameManager.Instance.ScreenEffects.FadeOut(time);
           // yield return new WaitForSeconds(time);
            //GameManager.Instance.ScreenEffects.FadeIn(time);
            Destroy(gameObject);
           // yield return new WaitForSeconds(time);
        }

        public override void Activate()
        {
            StartCoroutine(Explosions(GetComponent<Collider2D>()));
        }

        public override void Deactive()
        {
            
        }
    }
}
