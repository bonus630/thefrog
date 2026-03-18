using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.InputSystem;
namespace br.com.bonus630.thefrog.Items
{
    public class Checkpoint : MonoBehaviour
    {
        protected bool active = false;

        public bool InCheckPoint { get; protected set; }

        [SerializeField] InputAction SaveAction;

        public virtual void Start()
        {
            SaveAction.Enable();
        }
        public virtual void Update()
        {
            //if(InCheckPoint)
            if(SaveAction.WasReleasedThisFrame() && InCheckPoint)
            {

               // Debug.Log("checkpoint save menu call");
                GameManager.Instance.OnCallSave(true);
            }
        }
        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") )
            {
                InCheckPoint = true;
                if(!active)
                    Check();
            }
        }
        protected void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                InCheckPoint = false;
            }
        }
        protected virtual void Check()
        {
            // Debug.Log("base checkpoint");
            GameManager.Instance.PlayerStates = ServiceLocator.Instance.Get("Player").GetComponent<PlayerManager>().PlayerStates;
            GameManager.Instance.PlayerStates.PlayerPosition.Position = gameObject.transform.position;
            GameManager.Instance.SaveStates(0);
            active = true;
        } 
        protected virtual void UnCheck()
        {
            active = false;
        }
    }
}

