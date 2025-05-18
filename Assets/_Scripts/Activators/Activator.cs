using System.Collections;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Activator : MonoBehaviour
    {
        CircleCollider2D circleCollider;
        [SerializeField][Tooltip("Um IActivator item")] IActivator ItemToActive;
        [SerializeField][Tooltip("Um gameobject com multiplos IActivator ou para ativar e desativar")] GameObject GameObjectToActive;
        [SerializeField] float delayActiveTime = 0f;
        [SerializeField] float delayDeactiveTime = 0f;
        [SerializeField] bool permanentActived = false;
        void Start()
        {
            circleCollider = GetComponent<CircleCollider2D>();

        }


        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                StopAllCoroutines();
                StartCoroutine(ToggleActivations(true, delayActiveTime));
            }

        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (permanentActived)
                return;
            if (collision.CompareTag("Player"))
            {
                StopAllCoroutines();
                StartCoroutine(ToggleActivations(false, delayDeactiveTime));
            }
        }
        private IEnumerator ToggleActivations(bool active, float time)
        {
            yield return new WaitForSeconds(time);
            Debug.Log("time");
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
