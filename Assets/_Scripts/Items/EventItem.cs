using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class EventItem : MonoBehaviour
    {
        [SerializeField] GameEventName eventToComplete;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance.EventCompleted(eventToComplete);
                Destroy(gameObject);
            }
        }
    }
}
