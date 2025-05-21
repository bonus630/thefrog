using System.Collections;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class Lever : MonoBehaviour
    {

        [SerializeField] IActivator ItemToActive;
        [SerializeField] float delayTime;
        [SerializeField] bool actived = true;


        int OnID = Animator.StringToHash("On");
        private void Start()
        {
            GetComponent<Animator>().SetBool(OnID, actived);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GetComponent<AudioSource>().Play();
            if (actived)
            {
                StartCoroutine(TurnOff());
            }
            else
            {
                StartCoroutine(TurnOn());
            }
            actived = !actived;
            GetComponent<Animator>().SetBool(OnID,actived);
        }

        IEnumerator TurnOff()
        {
            yield return new WaitForSeconds(delayTime);
            ItemToActive.Deactive();
        }
        IEnumerator TurnOn()
        {
            yield return new WaitForSeconds(delayTime);
            ItemToActive.Activate();
        }
    }
}
