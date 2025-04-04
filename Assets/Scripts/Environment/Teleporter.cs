using System.Collections;
using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class Teleporter : IActivator
    {
        [SerializeField] GameObject teleported;
        [SerializeField] GameObject from;
        [SerializeField] GameObject to;
        [SerializeField] bool Auto;



        public override void Activate()
        {
            StartCoroutine(Acvation());
        }
        private IEnumerator Acvation()
        {


            SpriteRenderer render = null;
            Rigidbody2D rb = null;
            Player player = null;
            if (teleported.TryGetComponent<SpriteRenderer>(out render))
            {
                render.enabled = false;
            }
            if (teleported.TryGetComponent<Rigidbody2D>(out rb))
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            if (teleported.TryGetComponent<Player>(out player))
            {
                player.InputOn = false;
            }
            teleported.transform.position = to.transform.position;
            yield return new WaitForSeconds(0.1f);
            if (render != null)
                render.enabled = true;
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Dynamic;
            if(player!=null)
                player.InputOn = true; 


        }

        public override void Deactive()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(teleported.tag))
            {

                if (Auto)
                    Activate();
            }
        }
    }
}
