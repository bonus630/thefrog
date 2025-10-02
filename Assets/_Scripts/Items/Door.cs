using System.Collections;
using br.com.bonus630.thefrog.Manager;
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
        //protected InputSystem_Actions GlobalActions;
        protected Collider2D doorCollider;
        protected InputAction InteractUp;
        //[SerializeField] AudioClip openingAudio;
        //[SerializeField] AudioClip closingAudio;
        // [SerializeField] bool isOpen = true;
        //[SerializeField] bool isExit;

        //Animator anim;
        //AudioSource audioSource;
        //SpriteRenderer sprite;
        //GameObject door;
        //BoxCollider2D boxCollider;
        protected IPlayer player;

        private void Start()
        {
            var input = ServiceLocator.Get<PlayerInput>();
            var globalMap = input.actions.FindActionMap("Global", true);
            InteractUp = globalMap.FindAction("InteractUP", true);
            InteractUp.Enable();
            Debug.Log("InteractUO:" + InteractUp);
        }

        protected virtual void Awake()
        {
          
           // GlobalActions = new InputSystem_Actions();
           // GlobalActions.Enable();
            doorCollider = GetComponent<Collider2D>();
            //audioSource = GetComponent<AudioSource>();
            //anim = GetComponent<Animator>();
            //door = transform.GetChild(0).gameObject;
            //boxCollider = GetComponent<BoxCollider2D>();
        }

        protected virtual void Update()
        {
           // if (GlobalActions.Global.InteractUP.WasPressedThisFrame() && inside)
           if(InteractUp.WasPressedThisFrame() && inside)
            {
                var p = ServiceLocator.Get<IPlayer>();
                if (p.InGround && p.BodyTouching(doorCollider))
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
