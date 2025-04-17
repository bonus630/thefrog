using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class WorldLimit : MonoBehaviour
    {
        [SerializeField] CapsuleCollider2D player;
        [SerializeField] BoxCollider2D top;
        [SerializeField] BoxCollider2D down;
        BoxCollider2D left;
        BoxCollider2D right;


        private void Awake()
        {

        }
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {

            if (top.IsTouchingLayers(9))
            {
                Debug.Log("Touch");
                GameObject player = GameObject.Find("Player");
                player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                player.transform.position = new Vector3(player.transform.position.x, down.gameObject.transform.position.y + down.size.y, 0);
            }
        }
    }
}
