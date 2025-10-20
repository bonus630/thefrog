using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Items
{
    [RequireComponent(typeof(Collider2D))]
    public class Door : MonoBehaviour
    {
        [SerializeField] protected IActivator teleporter;
        [SerializeField] protected bool inside = false;
        protected Collider2D doorCollider;
        protected InputAction InteractUp;

        protected IPlayer player;

        private void Start()
        {
            var input = ServiceLocator.Instance.Get<PlayerInput>();
            var globalMap = input.actions.FindActionMap("Global", true);
            InteractUp = globalMap.FindAction("InteractUP", true);
            InteractUp.Enable();
            player = ServiceLocator.Instance.Get<IPlayer>();
        }

        protected virtual void Awake()
        {
            doorCollider = GetComponent<Collider2D>();
        }

        protected virtual void Update()
        {
            if (InteractUp.WasPressedThisFrame() && inside)
            {
                Debug.Log("[DoorBase] player:"+player);
                if (player.InGround && player.BodyTouching(doorCollider))
                    teleporter.Activate();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IPlayer>(out player))
                inside = true;
        }
        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            //  Debug.Log("doot trigger exit:" + collision.name);
            if (collision.CompareTag("Player") && player != null)
            {
                //    Debug.Log("doot trigger exit tag player: " + player.InGround);
                if (!player.BodyTouching(this.doorCollider))
                {
                    //       Debug.Log("doot trigger exit tag player body: " + player.InGround);
                    inside = false;
                    player = null;
                }
            }

        }

        private void OnDestroy()
        {
            InteractUp.Disable();
        }

    }
}
