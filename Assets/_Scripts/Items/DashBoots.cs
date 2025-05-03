using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class DashBoots : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameManager.Instance.EventCompleted(GameEventName.Dash);
            Destroy(gameObject);
        }
    }
}
