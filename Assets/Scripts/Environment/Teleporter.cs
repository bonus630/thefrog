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
            SpriteRenderer render = null;
            Rigidbody2D rb = null;
            if (teleported.TryGetComponent<SpriteRenderer>(out render))
            {
                render.enabled = false;
            }
            if (teleported.TryGetComponent<Rigidbody2D>(out rb))
            {
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            teleported.transform.position = to.transform.position;
            if (render != null)
                render.enabled = true;
            if (rb != null)
                rb.bodyType = RigidbodyType2D.Dynamic;



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
