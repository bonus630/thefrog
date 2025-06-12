using System.Collections;
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
        [SerializeField] float delayTime;
        bool cancel = false;
        
        public override void Activate()
        {
        
            StartCoroutine(Acvation());
        }
        private IEnumerator Acvation()
        {

            yield return new WaitForSeconds(delayTime);
            if(cancel)
                yield break;
            SpriteRenderer render = null;
            Rigidbody2D rb = null;
            IPlayer player = null;
            if (teleported.TryGetComponent<SpriteRenderer>(out render))
            {
                render.enabled = false;
            }
            if (teleported.TryGetComponent<Rigidbody2D>(out rb))
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            if (teleported.TryGetComponent<IPlayer>(out player))
            {
                player.InputsOn = false;
            }
            teleported.transform.position = to.transform.position;
            yield return new WaitForSeconds(0.1f);
            if (render != null)
                render.enabled = true;
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Dynamic;
            if(player!=null)
                player.InputsOn = true; 


        }

        public override void Deactive()
        {
            cancel = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (teleported == null)
                return;
            if (collision!=null && collision.CompareTag(teleported.tag))
            {
                cancel = false;
                if (Auto)
                    Activate();
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (teleported == null)
                return;
            if (collision != null && collision.CompareTag(teleported.tag))
            {

              Deactive();
            }
        }
    }
}
