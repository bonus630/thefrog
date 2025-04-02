using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Enemies
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Activator : MonoBehaviour
    {
        CircleCollider2D circleCollider;
        [SerializeField][Tooltip("Um IActivator item")] IActivator ItemToActive;

        void Start()
        {
            circleCollider = GetComponent<CircleCollider2D>();

        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                ItemToActive.Activate();
            }

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                ItemToActive.Deactive();
            }
        }
    }
}
