using br.com.bonus630.thefrog.Caracters;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class PlayerAlertPoints : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Player player;
            if (collision.gameObject.TryGetComponent<Player>(out player))
            {
                player.Alert();
            }
        }
    }
}
