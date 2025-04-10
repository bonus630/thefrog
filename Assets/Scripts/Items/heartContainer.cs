using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class heartContainer : MonoBehaviour
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.Instance.EventCompleted(GameEventName.HeartContainer);
                GameManager.Instance.UpdateHeart(1);
                Destroy(gameObject);
            }
        }
    }
}
