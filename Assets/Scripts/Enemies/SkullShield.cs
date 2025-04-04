using br.com.bonus630.thefrog.Caracters;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class SkullShield : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Player player = collision.gameObject.GetComponent<Player>();
                player.Hit();
                player.KnockUp(collision.GetContact(0).normal * 200);
                return;
            }
        }
    }
}
