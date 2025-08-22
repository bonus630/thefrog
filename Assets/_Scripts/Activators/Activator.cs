using System.Collections;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Collider2D))]
    public class Activator : MonoBehaviour
    {
        Collider2D _collider;
        [SerializeField][Tooltip("Um IActivator item")] IActivator ItemToActive;
        [SerializeField][Tooltip("Um gameobject com multiplos IActivator ou para ativar e desativar")] GameObject GameObjectToActive;
        [SerializeField] float delayActiveTime = 0f;
        [SerializeField] float delayDeactiveTime = 0f;
        [SerializeField] bool permanentActived = false;
        [SerializeField] bool onlyActive = false;
        [SerializeField] bool onlyDeactive = false;
       
        
        void Start()
        {
            _collider = GetComponent<Collider2D>();

        }
    
        void Reset()
        {
           // Debug.Log("Activator reset");
            _collider = GetComponent<Collider2D>();
            _collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (onlyDeactive)
                return;
            if (other.CompareTag("Player"))
            {
                StartCoroutine(true,delayActiveTime);
            }

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (permanentActived || onlyActive)
                return;
            if (collision.CompareTag("Player"))
            {
                StartCoroutine(false,delayDeactiveTime);
            }
        }
        private void StartCoroutine(bool active,float time)
        {
            StopAllCoroutines();
            StartCoroutine(ToggleActivations(active, time));
        }
        private IEnumerator ToggleActivations(bool active, float time)
        {
            yield return new WaitForSeconds(time);
          //  Debug.Log("time");
            if (ItemToActive != null)
                ToggleActivator(ItemToActive, active);
            if (GameObjectToActive != null)
            {
                IActivator[] activators = GameObjectToActive.GetComponents<IActivator>();
                if (activators.Length == 0)
                {
                    GameObjectToActive.SetActive(active);
                }
                else
                {
                    for (int i = 0; i < activators.Length; i++)
                    {
                        ToggleActivator(activators[i], active);
                    }
                }
            }
        }
        private void ToggleActivator(IActivator activator, bool active)
        {
            if (active)
                activator.Activate();
            else
                activator.Deactive();
        }
    }
}
