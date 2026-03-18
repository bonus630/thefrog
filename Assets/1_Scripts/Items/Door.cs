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
            ServiceLocator.Instance.GetAsync<IPlayer>(p => player = p);
        }

        protected virtual void Awake()
        {
            doorCollider = GetComponent<Collider2D>();
        }

        protected virtual void Update()
        {
            if (inside && InteractUp.WasPressedThisFrame())
            {
                player = ServiceLocator.Instance.Get<IPlayer>();
                //Debug.Log("[DoorBase] player:"+player);
                if (player.InGround && player.BodyTouching(doorCollider))
                    teleporter.Activate();
            }
        }
        //private int lastPlayerId = 0;

        //protected virtual void Update()
        //{
        //    if (InteractUp.WasPressedThisFrame() && inside)
        //    {
        //        if (player is MonoBehaviour mb)
        //        {
        //            if (mb == null)
        //            {
        //                Debug.LogWarning("[Door] Player foi destruído, referência inválida.");
        //                inside = false;
        //                player = null;
        //                return;
        //            }

        //            if (mb.GetInstanceID() != lastPlayerId)
        //            {
        //                Debug.Log($"[Door] Player mudou! ID antigo: {lastPlayerId}, novo: {mb.GetInstanceID()}");
        //                lastPlayerId = mb.GetInstanceID();
        //            }
        //        }

        //        Debug.Log($"[Door] Player hash: {player?.GetHashCode()}, ID: {((player as MonoBehaviour)?.GetInstanceID() ?? -1)}");

        //        if (player != null && player.InGround && player.BodyTouching(doorCollider))
        //            teleporter.Activate();
        //    }
        //}
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
                    //player = null;
                }
            }

        }

        private void OnDestroy()
        {
            InteractUp.Disable();
        }

    }
}
