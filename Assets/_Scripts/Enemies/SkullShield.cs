using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Enemies
{
    public class SkullShield :  IActivator
    {
        [SerializeField] Skull skullBody;
        public override void Activate()
        {
           
        }

        public override void Deactive()
        {
            skullBody.DisableShield();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("SkullShield: colis�o com " + collision.gameObject.name + ", shield ativo: " + gameObject.activeInHierarchy);
            if (collision.gameObject.CompareTag("Player"))
            {
                IPlayer player = collision.gameObject.GetComponent<IPlayer>();
                player.Hit();
                player.KnockUp(collision.GetContact(0).normal * 200);
                return;
            }
        }
    }
}
