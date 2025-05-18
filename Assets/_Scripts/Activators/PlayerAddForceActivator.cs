
using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class PlayerAddForceActivator : MonoBehaviour
    {
        [SerializeField] Vector2 force;
        private void OnTriggerExit2D(Collider2D collision)
        {
            if(collision.TryGetComponent<Player>(out Player player))
            {
                player.AddForce(force);
                gameObject.SetActive(false);
            }
        }
    }
}
