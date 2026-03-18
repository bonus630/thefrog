using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class PlayerAlertPoints : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            IPlayer player;
            if (collision.gameObject.TryGetComponent<IPlayer>(out player))
            {
                player.Alert();
            }
        }
    }
}
