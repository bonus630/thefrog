using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerActivator : MonoBehaviour
    {
        Collider2D _collider;
        [SerializeField][Tooltip("Um IActivator item")] IActivator ItemToActive;

        bool playerInCollider = false;
        bool actived = false;
        void Start()
        {
            _collider = GetComponent<Collider2D>();
        }
        


        private void OnTriggerEnter2D(Collider2D other)
        {

            if (other.TryGetComponent<IPlayer>(out IPlayer player) && player.BodyTouching(_collider))
            {
                if (playerInCollider)
                    return;
               // Debug.Log("[Activators][OnTriggerEnter2D] gameobject: " + gameObject.name+" other.tag: "+other.tag);
                if (!actived)
                {
                    actived = true;
                    ItemToActive.Activate();
                    playerInCollider = true;
                }
            }

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
        
            if (collision.CompareTag("Player"))
            {
                actived = false;
                ItemToActive.Deactive();
                playerInCollider = false;
            }
        }
   
       
    }
}
